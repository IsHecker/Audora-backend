namespace Audora.Contracts.Analytics.Responses;

public class EpisodeAnalyticsResponse
{
    public string EpisodeName { get; init; } = null!;
    public int PlayCount { get; init; }
    public int Likes { get; init; }
    public int Dislikes { get; init; }
    public int Comments { get; init; }
    public int Downloads { get; init; }
    
    public int? Replays { get; init; }
    public int? Shares { get; init; }
    public int? Bookmarks { get; init; }
    public long? ListeningTime { get; init; }
}

