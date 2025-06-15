using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Common;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Common.Enums;
using Audora.Application.Common.Mappings;
using Audora.Domain.Entities;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;

namespace Audora.Application.Podcasts.Queries.ListFollowedPodcasts;

public record ListFollowedPodcastsQuery(Guid ListenerId, Pagination Pagination) : IQuery<PagedResponse<PodcastResponse>>;

public class ListFollowedPodcastsQueryHandler : IQueryHandler<ListFollowedPodcastsQuery, PagedResponse<PodcastResponse>>
{
  private readonly IPodcastRepository _podcastRepository;
  private readonly IFollowRepository _followRepository;
  private readonly PodcastResponseAttacher _podcastResponseAttacher;

  public ListFollowedPodcastsQueryHandler(
      IFollowRepository followRepository,
      IPodcastRepository podcastRepository,
      PodcastResponseAttacher podcastResponseAttacher)
  {
    _followRepository = followRepository;
    _podcastRepository = podcastRepository;
    _podcastResponseAttacher = podcastResponseAttacher;
  }


  public async Task<Result<PagedResponse<PodcastResponse>>> Handle(ListFollowedPodcastsQuery request,
      CancellationToken cancellationToken)
  {
    var followedPodcastIds = (await _followRepository.GetListenerFollows(request.ListenerId, EntityType.Podcast))
        .Select(f => f.EntityId);

    var totalCount = followedPodcastIds.Count();

    if (totalCount == 0)
      return Array.Empty<PodcastResponse>().ToPagedResponse(request.Pagination, 0);


    followedPodcastIds = followedPodcastIds.Paginate(request.Pagination);


    var podcasts = await _podcastRepository.WithPublishedPodcasts().GetAllAsync();

    var followedPodcasts = podcasts.Where(podcast => followedPodcastIds.Contains(podcast.Id));

    return CreateResponse(request, followedPodcasts, totalCount);
  }

  private Result<PagedResponse<PodcastResponse>> CreateResponse(
      ListFollowedPodcastsQuery request,
      IQueryable<Podcast> followedPodcasts,
      int totalCount)
  {
    var response = followedPodcasts.ToResponse().ToList();

    _podcastResponseAttacher.AttachTo(response).AttachRatings(request.ListenerId);

    foreach (var item in response)
    {
      item.IsFollowing = true;
    }

    return response.ToPagedResponse(request.Pagination, totalCount);
  }
}