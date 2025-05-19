using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Common.Services;

// TODO CRITICAL manage response in a flexible and attaching way.
public class PodcastResponseAttacher : ResponseAttacher<PodcastResponseAttacher, PodcastResponse>
{
    private readonly IFollowRepository _followRepository;
    private readonly IPodcastRatingRepository _ratingRepository;

    private HashSet<Guid>? _podcastIdsCache;
    private HashSet<Guid> PodcastIds =>
        _podcastIdsCache ??= ResponseCollection.Select(p => p.Id).ToHashSet();


    public PodcastResponseAttacher(
        IFollowRepository followRepository,
        IPodcastRatingRepository ratingRepository)
    {
        _followRepository = followRepository;
        _ratingRepository = ratingRepository;
    }

    public PodcastResponseAttacher AttachRatings(Guid listenerId) =>
        AttachAsync(
            () => AttachRatingForOneAsync(listenerId),
            () => AttachRatingsForAllAsync(listenerId)
        ).GetAwaiter().GetResult();

    public PodcastResponseAttacher AttachFollowStatus(Guid listenerId) =>
        AttachAsync(
            () => AttachFollowStatusForOneAsync(listenerId),
            () => AttachFollowStatusForAllAsync(listenerId)
        ).GetAwaiter().GetResult();


    private async Task AttachRatingsForAllAsync(Guid listenerId)
    {
        var ratings = await _ratingRepository.GetAllByListenerIdAsync(listenerId);
        var ratingDict = ratings
            .Where(r => PodcastIds.Contains(r.PodcastId))
            .ToDictionary(r => r.PodcastId, r => r.Rating);

        AddAttachment(response =>
        {
            response.UserRating = ratingDict.TryGetValue(response.Id, out var rating) ? rating : null;
        });
    }

    private async Task AttachRatingForOneAsync(Guid listenerId)
    {
        var rating = (await _ratingRepository.GetAllByListenerIdAsync(listenerId))
            .FirstOrDefault(r => r.PodcastId == SingleResponse!.Id)?.Rating;

        SingleResponse!.UserRating = rating;
    }

    private async Task AttachFollowStatusForAllAsync(Guid listenerId)
    {
        var follows = await _followRepository.GetListenerFollowsByEntityIds(listenerId, PodcastIds);
        var followedIds = follows.Select(f => f.EntityId).ToHashSet();

        AddAttachment(response => response.IsFollowing = followedIds.Contains(response.Id));
    }

    private async Task AttachFollowStatusForOneAsync(Guid listenerId)
    {
        SingleResponse!.IsFollowing = await _followRepository.IsListenerFollowingAsync(listenerId, SingleResponse.Id);
    }
}