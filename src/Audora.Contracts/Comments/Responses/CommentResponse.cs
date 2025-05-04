using Audora.Contracts.Reactions.Responses;

namespace Audora.Contracts.Comments.Responses;

public class CommentResponse
{
    public Guid Id { get; init; }
    public Guid? ParentId { get; init; } = null!;
    public Guid? ListenerId { get; init; }
    
    public string Content { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string? UserAvatarUrl { get; init; }
    public DateTime PostedAt { get; init; }
    public DateTime? EditedAt { get; init; } = null!;

    public EngagementStatResponse Engagements { get; init; } = null!;

    public IEnumerable<CommentResponse> Replies { get; init; } = null!;
}