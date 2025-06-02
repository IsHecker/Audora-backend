using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class PodcastStat : Entity
{
    private readonly TimeSpan _returningListenerTimeSpan = TimeSpan.FromSeconds(20); // normally it's 30 days.

    public Guid PodcastId { get; init; }
    public string PodcastName { get; private set; } = null!;
    public float AverageRating { get; private set; } = 0;
    public int TotalRatings { get; private set; } = 0;
    public long TotalPlays { get; private set; } = 0;
    public long TotalFollowers { get; private set; } = 0;
    public int TotalReturningListeners { get; private set; } = 0;
    public float RetentionRate { get; private set; } = 0;
    public long TotalListeningTime { get; private set; } = 0;

    public Podcast Podcast { get; init; } = null!;

    public PodcastStat(Guid podcastId, string podcastName)
    {
        PodcastId = podcastId;
        PodcastName = podcastName;
    }

    private PodcastStat()
    {
    }

    public void AddRating(byte rating)
    {
        var ratingsSum = (AverageRating * TotalRatings) + rating;
        AverageRating = float.Round(ratingsSum / ++TotalRatings, 1);
    }

    public void ReplaceListenerRating(byte oldRating, byte newRating)
    {
        var ratingsSum = AverageRating * TotalRatings - oldRating + newRating;
        AverageRating = float.Round(ratingsSum / TotalRatings, 1);

        if (newRating == 0) // removing rate.
        {
            TotalRatings--;
        }
    }

    public void ChangePodcastName(string newPodcastName) => PodcastName = newPodcastName;

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