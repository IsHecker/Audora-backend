using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(Comment Comment) : ICommand;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;
    private readonly ICommentStatRepository _commentStatRepository;

    public CreateCommentCommandHandler(
        ICommentRepository commentRepository,
        ICommentStatRepository commentStatRepository)
    {
        _commentRepository = commentRepository;
        _commentStatRepository = commentStatRepository;
    }

    public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = request.Comment;

        // If this is a reply to another comment, ensure that comment's stat exists
        if (comment.ParentId.HasValue)
        {
            if (!await _commentRepository.ExistsAsync(comment.ParentId.Value))
                return Error.Failure("Comment.NotFound", "Parent comment does not exist.");

            var parentCommentStat = (await _commentStatRepository.AsTracking().GetCommentStatAsync(comment.ParentId.Value, EntityType.Comment))
            ?? await _commentStatRepository.AddAsync(comment.ParentId.Value, EntityType.Comment);

            parentCommentStat.IncreaseCommentCount();
        }

        await _commentRepository.AddAsync(comment);

        // Get or create stat for the entity being commented on
        var commentStat = (await _commentStatRepository.AsTracking().GetCommentStatAsync(comment.EntityId, comment.EntityType))
            ?? await _commentStatRepository.AddAsync(comment.EntityId, comment.EntityType);

        commentStat.IncreaseCommentCount();

        return Result.Success;
    }
}