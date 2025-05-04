using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Comments.Commands.CreateComment;

public record CreateCommentCommand(Comment Comment) : ICommand;

public class CreateCommentCommandHandler : ICommandHandler<CreateCommentCommand>
{
    private readonly ICommentRepository _commentRepository;

    public CreateCommentCommandHandler(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public async Task<Result> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        await _commentRepository.AddAsync(request.Comment);
        return Result.Success;
    }
}