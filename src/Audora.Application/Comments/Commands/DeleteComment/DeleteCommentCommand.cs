using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Comments.Commands.DeleteComment;

public record DeleteCommentCommand(Guid CommentId) : ICommand;

public class DeleteCommentCommandHandler : ICommandHandler<DeleteCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public DeleteCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        await _commentRepository.DeleteAsync(request.CommentId);
        return Result.Success;
    }
}