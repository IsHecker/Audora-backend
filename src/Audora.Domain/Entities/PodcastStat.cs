using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PodcastStat : Entity
{
    private readonly TimeSpan _returningListenerTimeSpan = TimeSpan.FromSeconds(20); // normally it's 30 days.

    public Guid PodcastId { get; init; }
    public float AverageRating { get; private set; }
    public int TotalRatings { get; private set; }
    public long TotalPlays { get; private set; }
    public long TotalFollowers { get; private set; }
    public int TotalReturningListeners { get; private set; }
    public float RetentionRate { get; private set; }
    public long TotalListeningTime { get; private set; }

    public Podcast Podcast { get; init; } = null!;
    // public ICollection<EpisodeStat> EpisodesStats { get; private set; } = [];

    public PodcastStat(Guid podcastId, float averageRating, int totalRatings, long totalPlays, long totalFollowers,
        float retentionRate, long totalListeningTime)
    {
        PodcastId = podcastId;
        AverageRating = averageRating;
        TotalRatings = totalRatings;
        TotalPlays = totalPlays;
        TotalFollowers = totalFollowers;
        RetentionRate = retentionRate;
        TotalListeningTime = totalListeningTime;
    }

    private PodcastStat()
    {
    }

    public void AddRating(byte rating)
    {
        var ratingsSum = (AverageRating * TotalRatings) + rating;
        AverageRating = float.Round(ratingsSum / ++TotalRatings, 1);
    }

    public void ReplaceRating(byte oldRating, byte newRating)
    {
        var ratingsSum = AverageRating * TotalRatings - oldRating + newRating;
        AverageRating = float.Round(ratingsSum / TotalRatings, 1);

        if (newRating == 0) // removing rate.
        {
            TotalRatings--;
        }
    }

    public void AddFollower() => TotalFollowers++;

    public void RemoveFollower() => TotalFollowers--;

    public void IncreaseTotalPlays() => TotalPlays++;

    public void CalculateRetentionRate(DateTime lastVisit)
    {
        if (DateTime.Now - lastVisit > _returningListenerTimeSpan)
        {
            TotalReturningListeners++;
        }

        RetentionRate = (float)TotalReturningListeners / TotalPlays * 100;
    }

    public void UpdateTotalListeningTime(long episodeListeningTime) => TotalListeningTime += episodeListeningTime;
}