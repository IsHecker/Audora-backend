using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
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
        var follow = (await _followRepository.GetUserFollows(request.FollowerId))
            .FirstOrDefault(f => f.EntityId == request.PodcastId);

        var podcastStat = await _podcastStatRepository.GetPodcastStatByPodcastIdAsync(request.PodcastId);

        if (follow is null)
        {
            await _followRepository.AddAsync(request.FollowerId, request.PodcastId);
            podcastStat.AddFollower();
            
            return Result.Success;
        }
        
        await _followRepository.DeleteFollowAsync(request.FollowerId, request.PodcastId);
        podcastStat.RemoveFollower();
        
        return Result.Success;
    }
}