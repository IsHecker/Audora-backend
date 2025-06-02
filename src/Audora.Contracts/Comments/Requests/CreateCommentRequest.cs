namespace Audora.Contracts.Comments.Requests;

public class CreateCommentRequest
{
  public Guid? ParentId { get; init; }
  public string Content { get; init; } = null!;
}