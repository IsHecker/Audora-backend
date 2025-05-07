using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Follows.Commands.ToggleFollow;

public record TogglePodcastFollowCommand(Guid FollowerId, Guid PodcastId) : ICommand;

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
        var listenerFollow = (await _followRepository.GetListenerFollows(request.FollowerId))
            .FirstOrDefault(f => f.EntityId == request.PodcastId);

        var podcastStat = await _podcastStatRepository.GetByPodcastIdAsync(request.PodcastId);

        if (listenerFollow is null)
        {
            var newFollow = new Follow(request.FollowerId, request.PodcastId, FollowTarget.Podcast);

            await _followRepository.AddAsync(newFollow);
            podcastStat.AddFollower();

            return Result.Success;
        }

        await _followRepository.DeleteAsync(listenerFollow);
        podcastStat.RemoveFollower();

        return Result.Success;
    }
}