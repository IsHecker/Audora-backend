using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Ratings.Commands.RatePodcast;

public record RatePodcastCommand(Guid PodcastId, Guid ListenerId, byte Rating) : ICommand;

public class RatePodcastCommandHandler : ICommandHandler<RatePodcastCommand>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IPodcastRating _podcastRating;

    public RatePodcastCommandHandler(IPodcastRepository podcastRepository, IPodcastStatRepository podcastStatRepository,
        IPodcastRating podcastRating)
    {
        _podcastRepository = podcastRepository;
        _podcastStatRepository = podcastStatRepository;
        _podcastRating = podcastRating;
    }

    public async Task<Result> Handle(RatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository.GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        var listenerRating = await _podcastRating.GetListenerRatingByPodcastIdAsync(request.PodcastId);
        var podcastStat = await _podcastStatRepository.GetPodcastStatByPodcastIdAsync(request.PodcastId);

        if (listenerRating is null)
        {
            await _podcastRating.AddAsync(request.PodcastId, request.ListenerId, request.Rating);
            podcastStat.AddRating(request.Rating);
            podcast.UpdateAverageRating(podcastStat.AverageRating);

            return Result.Success;
        }

        podcastStat.ReplaceRating(listenerRating.Rating, request.Rating);
        podcast.UpdateAverageRating(podcastStat.AverageRating);

        return Result.Success;
    }
}