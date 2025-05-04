namespace Audora.Contracts.Reactions.Responses;

public class EpisodeEngagementStatResponse : EngagementStatResponse
{
    public int PlayCount { get; init; }
    public int Shares { get; init; }
    public int Bookmarks { get; init; }
    public bool IsTrending { get; init; }
}