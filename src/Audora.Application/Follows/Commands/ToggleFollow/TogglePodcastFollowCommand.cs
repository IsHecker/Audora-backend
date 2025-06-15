using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Follows.Commands.ToggleFollow;

public record TogglePodcastFollowCommand(Guid ListenerId, Guid PodcastId, EntityType EntityType) : ICommand;

public class TogglePodcastFollowCommandHandler : ICommandHandler<TogglePodcastFollowCommand>
{
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IFollowRepository _followRepository;

    public TogglePodcastFollowCommandHandler(IFollowRepository followRepository,
        IPodcastStatRepository podcastStatRepository)
    {
        _followRepository = followRepository;
        _podcastStatRepository = podcastStatRepository;
    }

    public async Task<Result> Handle(TogglePodcastFollowCommand request, CancellationToken cancellationToken)
    {
        // TODO: modify to be on all entities not just podcast.

        var podcastStat = await _podcastStatRepository.AsTracking().GetByPodcastIdAsync(request.PodcastId);
        if (podcastStat is null)
            return Error.NotFound(description: $"PodcastStat with podcast Id '{request.PodcastId} is not found.'");

        var listenerFollow = (await _followRepository.GetListenerFollows(request.ListenerId, request.EntityType))
            .FirstOrDefault(f => f.EntityId == request.PodcastId);

        if (listenerFollow is null)
        {
            var newFollow = new Follow(request.ListenerId, request.PodcastId, EntityType.Podcast);

            await _followRepository.AddAsync(newFollow);
            podcastStat.AddFollower();

            return Result.Success;
        }

        await _followRepository.DeleteAsync(listenerFollow);
        podcastStat.RemoveFollower();

        return Result.Success;
    }
}