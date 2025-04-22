namespace Audora.Domain.Entities;

public class PodcastStat
{
    public Guid PodcastId { get; init; }
    public decimal AverageRating { get; init; }
    public int TotalRatings { get; init; }

    public PodcastStat(Guid podcastId, decimal averageRating, int totalRatings)
    {
        PodcastId = podcastId;
        AverageRating = averageRating;
        TotalRatings = totalRatings;
    }

    private PodcastStat()
    {
    }
}