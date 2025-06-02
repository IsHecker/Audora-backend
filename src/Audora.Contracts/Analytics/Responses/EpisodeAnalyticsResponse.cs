namespace Audora.Contracts.Analytics.Responses;

public class EpisodeStatsResponse
{
    public string EpisodeName { get; init; } = null!;
    public int PlayCount { get; init; }
    public int Downloads { get; init; }
    public int Comments { get; init; }

    public int? Replays { get; init; }
    public int? Shares { get; init; }
    public int? Bookmarks { get; init; }
    public long? ListeningTime { get; init; }

    public Dictionary<string, int>? Reactions { get; init; } = null!;
}