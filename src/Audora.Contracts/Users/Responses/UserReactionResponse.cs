using Audora.Contracts.Reactions.Responses;

namespace Audora.Contracts.Users.Responses;

public class UserReactionResponse
{
    public UserResponse User { get; init; } = null!;
    public ReactionResponse Reaction { get; init; } = null!;
}