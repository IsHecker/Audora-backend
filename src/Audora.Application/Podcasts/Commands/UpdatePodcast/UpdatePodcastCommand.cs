using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Commands.UpdatePodcast;

public record UpdatePodcastCommand(Guid PodcastId, Podcast Podcast) : ICommand;

public class UpdatePodcastCommandHandler : ICommandHandler<UpdatePodcastCommand>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IPodcastStatRepository _podcastStatRepository;

    public UpdatePodcastCommandHandler(
        IPodcastRepository podcastRepository,
        IPodcastStatRepository podcastStatRepository)
    {
        _podcastRepository = podcastRepository;
        _podcastStatRepository = podcastStatRepository;
    }

    public async Task<Result> Handle(UpdatePodcastCommand request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository.AsTracking().GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        var updatedPodcast = request.Podcast;

        podcast.Update(updatedPodcast);

        if (podcast.Name != updatedPodcast.Name)
        {
            var podcastStat = await _podcastStatRepository.AsTracking().GetByPodcastIdAsync(podcast.Id);
            podcastStat?.ChangePodcastName(podcast.Name);
        }

        return Result.Success;
    }
}