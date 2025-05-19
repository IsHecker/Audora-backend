using Audora.Contracts.Podcasts.Requests;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PodcastMapping
{
    private static HashSet<Guid>? _isFollowedSet;
    private static Dictionary<Guid, byte>? _userRatingDict;


    public static Podcast ToDomain(this CreatePodcastRequest request)
    {
        return Podcast.Create(
            name: request.Name,
            description: request.Description,
            isPublished: request.IsPublished,
            category: request.Category,
            language: request.Language,
            tags: request.Tags,
            creatorId: request.CreatorId,
            coverImageUrl: request.CoverImageUrl);
    }

    public static Podcast ToDomain(this UpdatePodcastRequest request)
    {
        return Podcast.Create(
            name: request.Name,
            description: request.Description,
            isPublished: request.IsPublished,
            category: request.Category,
            language: request.Language,
            tags: request.Tags,
            creatorId: Guid.Empty,
            coverImageUrl: request.CoverImageUrl);
    }

    public static PodcastResponse ToResponse(this Podcast podcast, bool? isFollowed = null, byte? userRating = null)
    {
        return new PodcastResponse
        {
            Id = podcast.Id,
            CreatorId = podcast.CreatorId,
            Name = podcast.Name,
            Description = podcast.Description,
            CoverImageUrl = podcast.CoverImageUrl,
            Category = podcast.Category,
            Language = podcast.Language,
            TotalEpisodes = podcast.TotalEpisodes,
            AverageRating = podcast.AverageRating,
            TotalRatings = podcast.TotalRatings,
            IsFollowing = isFollowed,
            UserRating = userRating,
        };
    }

    public static IEnumerable<PodcastResponse> ToResponse(this IEnumerable<Podcast> podcasts)
    {
        var response = podcasts.Select(p =>
        {
            var isFollowed = _isFollowedSet?.Contains(p.Id);

            byte? userRating = null;
            if (_userRatingDict != null && _userRatingDict.TryGetValue(p.Id, out var rating))
                userRating = rating;

            return p.ToResponse(isFollowed, userRating);
        });

        Reset();

        return response;
    }

    public static IQueryable<Podcast> WithListenerMetaData(this IQueryable<Podcast> podcasts,
        HashSet<Guid> isFollowedSet,
        Dictionary<Guid, byte> userRatingDict)
    {
        _isFollowedSet = isFollowedSet;
        _userRatingDict = userRatingDict;
        return podcasts;
    }

    private static void Reset()
    {
        _isFollowedSet = null;
        _userRatingDict = null;
    }
}