using Audora.Contracts.Analytics.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PodcastAnalyticsMapping
{
    public static PodcastStatsResponse ToResponse(this PodcastStat podcastStat)
    {
        return new PodcastStatsResponse
        {
            PodcastName = podcastStat.PodcastName,
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