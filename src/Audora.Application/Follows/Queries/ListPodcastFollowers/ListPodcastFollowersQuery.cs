using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.Users.Responses;

namespace Audora.Application.Follows.Queries.ListPodcastFollowers;

public record ListPodcastFollowersQuery(Guid PodcastId, Pagination Pagination) : IQuery<PagedResponse<UserResponse>>;

public class ListPodcastFollowersQueryHandler : IQueryHandler<ListPodcastFollowersQuery, PagedResponse<UserResponse>>
{
    private readonly IFollowRepository _followRepository;
    private readonly IUserService _userService;

    public ListPodcastFollowersQueryHandler(IFollowRepository followRepository, IUserService userService)
    {
        _followRepository = followRepository;
        _userService = userService;
    }


    public async Task<Result<PagedResponse<UserResponse>>> Handle(ListPodcastFollowersQuery request,
        CancellationToken cancellationToken)
    {
        var followerIds = (await _followRepository.GetAllByEntityIdAsync(request.PodcastId))
            .Select(f => f.FollowerId);

        var users = await _userService.GetUsersByIdsAsync(followerIds.Paginate(request.Pagination));

        return users.ToResponse().ToPagedResponse(request.Pagination, followerIds.Count());


        //return (await _userService.GetUsersByIdsAsync(followerIds)).ToResult<IEnumerable<User>>();
    }
}