using Audora.Contracts.Reactions.Responses;

namespace Audora.Contracts.EngagementStats.Responses;

public class EngagementStatsResponse
{
    public Dictionary<string, int>? Reactions { get; init; }
    public int? CommentCount { get; init; }

    public int? ShareCount { get; init; }      // Optional
    public int? BookmarkCount { get; init; }   // Optional
    public int? PlayCount { get; init; }       // Optional
    public ReactionResponse? ListenerReaction { get; set; }
}