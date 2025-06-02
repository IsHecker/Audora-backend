using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(Guid CommentId, Guid EntityId, EntityType EntityType) : ICommand;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentStatRepository _commentStatRepository;

    public DeleteCommentCommandHandler(
      ICommentRepository commentRepository,
      ICommentStatRepository commentStatRepository)
    {
        _commentRepository = commentRepository;
        _commentStatRepository = commentStatRepository;
    }

    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var commentStat = await _commentStatRepository.AsTracking().GetCommentStatAsync(request.EntityId, request.EntityType);

        if (commentStat is null)
            return Error.NotFound(description: $"CommentStat with EntityId '{request.EntityId}' is not found.");

        if (commentStat.CommentCount < 1)
            return Error.NotFound(description: $"No more Comments to remove.");

        var isDeleted = await _commentRepository.DeleteAsync(request.CommentId);

        if (!isDeleted)
            return Error.NotFound(description: $"Comment with Id '{request.CommentId}' is not found.");


        commentStat.DecreaseCommentCount();

        return Result.Success;
    }
}