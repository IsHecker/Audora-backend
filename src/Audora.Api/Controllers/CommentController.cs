using Audora.Application.Comments.Commands.CreateComment;
using Audora.Application.Comments.Commands.DeleteComment;
using Audora.Application.Comments.Queries.ListComments;
using Audora.Application.Common;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Contracts.Comments.Requests;
using Audora.Domain.Common.Enums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class CommentController : ApiController
{
    private readonly ISender _sender;

    public CommentController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiEndpoints.Comments.ListResourceComments)]
    public async Task<IActionResult> ListComments(Guid entityId, string resourceType, [FromQuery] Pagination pagination)
    {
        var query = new ListCommentsQuery(ListenerId, entityId, null, resourceType.ToEntityType(), pagination);
        var listCommentsResult = await _sender.Send(query);
        return listCommentsResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Comments.ListCommentReplies)]
    public async Task<IActionResult> ListCommentReplies(Guid parentId, [FromQuery] Pagination pagination)
    {
        var query = new ListCommentsQuery(ListenerId, null, parentId, EntityType.Comment, pagination);
        var listCommentsResult = await _sender.Send(query);
        return listCommentsResult.Match(Ok, Problem);
    }

    [HttpPost(ApiEndpoints.Comments.CommentOnEntity)]
    public async Task<IActionResult> CommentOnEntity(Guid entityId, string resourceType, CreateCommentRequest request)
    {
        var comment = request.ToDomain(entityId, ListenerId, resourceType.ToEntityType());
        var command = new CreateCommentCommand(comment);
        var createCommentResult = await _sender.Send(command);
        return createCommentResult.Match(Created, Problem);
    }

    [HttpDelete(ApiEndpoints.Comments.Delete)]
    public async Task<IActionResult> DeleteComment(Guid commentId, Guid entityId, EntityType entityType)
    {
        var command = new DeleteCommentCommand(commentId, entityId, entityType);
        var createCommentResult = await _sender.Send(command);
        return createCommentResult.Match(NoContent, Problem);
    }
}