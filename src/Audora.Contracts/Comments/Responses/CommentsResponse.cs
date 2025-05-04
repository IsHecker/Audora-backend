namespace Audora.Contracts.Comments.Responses;

public class CommentsResponse
{
    public IEnumerable<CommentResponse> Comments { get; set; } = null!;
}