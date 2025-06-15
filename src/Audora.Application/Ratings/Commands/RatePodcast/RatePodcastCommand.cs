using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Ratings.Commands.RatePodcast;

public record RatePodcastCommand(Guid PodcastId, Guid ListenerId, byte Rating) : ICommand;

public class RatePodcastCommandHandler : ICommandHandler<RatePodcastCommand>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IPodcastRatingRepository _podcastRatingRepository;

    public RatePodcastCommandHandler(IPodcastRepository podcastRepository, IPodcastStatRepository podcastStatRepository,
        IPodcastRatingRepository podcastRatingRepository)
    {
        _podcastRepository = podcastRepository;
        _podcastStatRepository = podcastStatRepository;
        _podcastRatingRepository = podcastRatingRepository;
    }

    public async Task<Result> Handle(RatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcastId = request.PodcastId;
        var podcast = await _podcastRepository.AsTracking().GetByIdAsync(podcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{podcastId}' is not found.");
        }

        var listenerRating = await _podcastRatingRepository.AsTracking().GetByEntityIdAsync(podcastId);
        var podcastStat = await _podcastStatRepository.AsTracking().GetByPodcastIdAsync(podcastId);

        if (listenerRating is null)
        {
            await AddRating(request, podcastId, podcast, podcastStat!);
            return Result.Success;
        }

        podcastStat!.ReplaceListenerRating(listenerRating.Rating, request.Rating);
        podcast.ChangeRating(podcastStat.AverageRating, podcastStat.TotalRatings);

        await UpdateOrRemoveListenerRatingAsync(listenerRating, request.Rating);

        return Result.Success;
    }

    private async Task AddRating(RatePodcastCommand request, Guid podcastId, Podcast podcast, PodcastStat podcastStat)
    {
        var newRating = new PodcastRating(podcastId, request.ListenerId, request.Rating);

        await _podcastRatingRepository.AddAsync(newRating);

        podcastStat!.AddRating(request.Rating);
        podcast.ChangeRating(podcastStat.AverageRating, podcastStat.TotalRatings);
    }

    private async Task UpdateOrRemoveListenerRatingAsync(PodcastRating listenerRating, byte newRating)
    {
        if (newRating < 1)
        {
            await _podcastRatingRepository.DeleteAsync(listenerRating);
            return;
        }

        listenerRating.ChangeRating(newRating);
    }
}