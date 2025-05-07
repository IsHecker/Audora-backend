using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using MediatR;

namespace Audora.Application.Follows.Queries.ListPodcastFollowers;

public record ListPodcastFollowersQuery(Guid PodcastId) : IQuery<IEnumerable<User>>;

public class ListPodcastFollowersQueryHandler : IQueryHandler<ListPodcastFollowersQuery, IEnumerable<User>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserService _userService;

    public ListPodcastFollowersQueryHandler(IFollowRepository followRepository, IUserService userService)
    {
        _followRepository = followRepository;
        _userService = userService;
    }


    public async Task<Result<IEnumerable<User>>> Handle(ListPodcastFollowersQuery request,
        CancellationToken cancellationToken)
    {
        var followerIds = (await _followRepository.GetAllByEntityIdAsync(request.PodcastId))
            .Select(f => f.FollowerId);
        
        return (await _userService.GetUsersByIdsAsync(followerIds)).ToResult<IEnumerable<User>>();
    }
}