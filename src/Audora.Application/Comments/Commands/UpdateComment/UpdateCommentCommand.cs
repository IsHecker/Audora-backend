using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Comments.Commands.UpdateComment;

public record UpdateCommentCommand(Guid CommentId, string Content) : ICommand;

public class UpdateCommentCommandHandler : ICommandHandler<UpdateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public UpdateCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
    {
        var comment = await _commentRepository.GetByIdAsync(request.CommentId);

        if (comment is null)
            return Error.NotFound(description: $"Comment with Id '{request.CommentId}' is not found.");

        comment.EditContent(request.Content);

        return Result.Success;
    }
}