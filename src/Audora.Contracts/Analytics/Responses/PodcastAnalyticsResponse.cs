namespace Audora.Contracts.Analytics.Responses;

public class PodcastAnalyticsResponse
{
    public string PodcastName { get; init; } = null!;
    public float AverageRating { get; init; }
    public int TotalRatings { get; init; }
    public long TotalPlays { get; init; }
    public long TotalFollowers { get; init; }
    public int TotalReturningListeners { get; init; }
    public float RetentionRate { get; init; }
    public long TotalListeningTime { get; init; }
    public IEnumerable<EpisodeAnalyticsResponse>? EpisodesAnalytics { get; init; }
}