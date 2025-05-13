using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Mappings;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Services;

public class PodcastService
{
    private readonly IFollowRepository _followRepository;
    private readonly IPodcastRatingRepository _ratingRepository;

    public PodcastService(IFollowRepository followRepository, IPodcastRatingRepository ratingRepository)
    {
        _followRepository = followRepository;
        _ratingRepository = ratingRepository;
    }

    public async Task<PodcastsResponse> AttachListenerMetadataAsync(IQueryable<Podcast> podcasts, Guid listenerId)
    {
        var podcastIds = podcasts.Select(p => p.Id);

        var isFollowingSet = (await _followRepository.GetListenerFollowsByEntityIds(listenerId, podcastIds))
            .Select(f => f.EntityId).ToHashSet();

        var userRatingDict = (await _ratingRepository.GetAllByListenerIdAsync(listenerId))
            .Where(r => podcastIds.Contains(r.PodcastId))
            .ToDictionary(r => r.PodcastId, r => r.Rating);

        return podcasts.WithListenerMetaData(isFollowingSet, userRatingDict).ToResponse();
    }
}