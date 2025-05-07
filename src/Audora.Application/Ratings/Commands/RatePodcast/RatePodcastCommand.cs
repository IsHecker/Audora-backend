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
        var podcast = await _podcastRepository.GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        var listenerRating = await _podcastRatingRepository.GetByEntityIdAsync(request.PodcastId);
        var podcastStat = await _podcastStatRepository.GetByPodcastIdAsync(request.PodcastId);

        if (listenerRating is null)
        {
            var podcastRating = new PodcastRating(request.PodcastId, request.ListenerId, request.Rating);
            
            await _podcastRatingRepository.AddAsync(podcastRating);
            podcastStat.AddRating(request.Rating);
            podcast.UpdateAverageRating(podcastStat.AverageRating);

            return Result.Success;
        }

        podcastStat.ReplaceRating(listenerRating.Rating, request.Rating);
        podcast.UpdateAverageRating(podcastStat.AverageRating);

        return Result.Success;
    }
}