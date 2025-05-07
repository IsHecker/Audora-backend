using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Follows.Queries.ListListenerPodcasts;

public record ListListenerPodcastsQuery(Guid ListenerId, Pagination Pagination) : IQuery<PodcastsResponse>;

public class ListListenerPodcastsQueryHandler : IQueryHandler<ListListenerPodcastsQuery, PodcastsResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IFollowRepository _followRepository;
    private readonly PodcastViewService _podcastViewService;

    public ListListenerPodcastsQueryHandler(IFollowRepository followRepository, IPodcastRepository podcastRepository,
        PodcastViewService podcastViewService)
    {
        _followRepository = followRepository;
        _podcastRepository = podcastRepository;
        _podcastViewService = podcastViewService;
    }


    public async Task<Result<PodcastsResponse>> Handle(ListListenerPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var followedPodcastIds = (await _followRepository.GetListenerFollows(request.ListenerId))
            .Where(f => f.FollowTarget == FollowTarget.Podcast)
            .Select(f => f.EntityId)
            .Paginate(request.Pagination);

        var podcasts = await _podcastRepository.GetAllAsync();

        // TODO check for errors.

        return await _podcastViewService.AttachListenerMetadataAsync(
            podcasts.Where(podcast => followedPodcastIds.Contains(podcast.Id)), 
            request.ListenerId);
    }
}