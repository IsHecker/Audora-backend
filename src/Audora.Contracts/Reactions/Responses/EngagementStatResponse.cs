namespace Audora.Contracts.Reactions.Responses;

public class EngagementStatResponse
{
    public int Likes { get; init; }
    public int Dislikes { get; init; }
    public int Comments { get; init; }
    public ListenerEntityReactionResponse? ListenerReaction { get; init; } = null!;
}