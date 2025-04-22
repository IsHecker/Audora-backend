using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class EpisodeStat : Entity
{
    public Guid EpisodeId { get; init; }

    public int TotalListenTime { get; init; }
    public int PlayCount { get; init; }
    public int Likes { get; init; }
    public int Dislikes { get; init; }
    public int Comments { get; init; }
    public int SkipCount { get; init; }
    public int Replays { get; init; }
    public int Downloads { get; init; }

    public decimal CompletionRate { get; init; }
    public decimal AverageListenDuration { get; init; }

    public EpisodeStat(
        Guid episodeId,
        int totalListenTime = 0,
        int playCount = 0,
        int likes = 0,
        int dislikes = 0,
        int comments = 0,
        int skipCount = 0,
        int replays = 0,
        int downloads = 0,
        decimal completionRate = 0,
        decimal averageListenDuration = 0)
    {
        EpisodeId = episodeId;
        TotalListenTime = totalListenTime;
        PlayCount = playCount;
        Likes = likes;
        Dislikes = dislikes;
        Comments = comments;
        SkipCount = skipCount;
        Replays = replays;
        Downloads = downloads;
        CompletionRate = completionRate;
        AverageListenDuration = averageListenDuration;
    }

    private EpisodeStat()
    {
    }
}