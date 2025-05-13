using Audora.Contracts.Analytics.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PodcastAnalyticsMapping
{
    public static PodcastAnalyticsResponse ToResponse(this PodcastStat podcastStat)
    {
        return new PodcastAnalyticsResponse
        {
            PodcastName = podcastStat.Podcast.Name,
            AverageRating = podcastStat.AverageRating,
            RetentionRate = podcastStat.RetentionRate,
            TotalFollowers = podcastStat.TotalFollowers,
            TotalListeningTime = podcastStat.TotalListeningTime,
            TotalPlays = podcastStat.TotalPlays,
            TotalRatings = podcastStat.TotalRatings,
            TotalReturningListeners = podcastStat.TotalReturningListeners,
        };
    }
}