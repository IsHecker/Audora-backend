using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Follows.Queries.ListFollowedPodcasts;

public record ListFollowedPodcastsQuery(Guid ListenerId, Pagination Pagination) : IQuery<PodcastsResponse>;

public class ListFollowedPodcastsQueryHandler : IQueryHandler<ListFollowedPodcastsQuery, PodcastsResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IFollowRepository _followRepository;
    private readonly PodcastService _podcastService;

    public ListFollowedPodcastsQueryHandler(IFollowRepository followRepository, IPodcastRepository podcastRepository,
        PodcastService podcastService)
    {
        _followRepository = followRepository;
        _podcastRepository = podcastRepository;
        _podcastService = podcastService;
    }


    public async Task<Result<PodcastsResponse>> Handle(ListFollowedPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var followedPodcastIds = (await _followRepository.GetListenerFollows(request.ListenerId))
            .Where(f => f.FollowTarget == FollowTarget.Podcast)
            .Select(f => f.EntityId)
            .Paginate(request.Pagination);

        var podcasts = await _podcastRepository.GetAllAsync();

        // TODO check for errors.

        var followedPodcasts = podcasts.Where(podcast => followedPodcastIds.Contains(podcast.Id));

        return await _podcastService.AttachListenerMetadataAsync(followedPodcasts, request.ListenerId);
    }
}