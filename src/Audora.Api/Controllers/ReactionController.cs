using Audora.Application.Common;
using Audora.Application.Common.Models;
using Audora.Application.Reactions.Commands.ToggleReaction;
using Audora.Application.Reactions.Queries.GetListenerReaction;
using Audora.Application.Reactions.Queries.ListEntityReactions;
using Audora.Contracts.Reactions.Requests;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class ReactionController : ApiController
{
    private readonly ISender _sender;

    public ReactionController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiEndpoints.Listeners.GetListenerReactionForEntity)]
    public async Task<IActionResult> GetListenerReactionForEntity(Guid listenerId, Guid entityId, EntityType entityType)
    {
        var query = new GetListenerReactionQuery(listenerId, entityId, entityType);
        var listenerReactionResult = await _sender.Send(query);
        return listenerReactionResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Reactions.ListEntityReactions)]
    public async Task<IActionResult> ListEntityReactions(Guid entityId, string resourceType, [FromQuery] Pagination pagination)
    {
        var query = new ListEntityReactionsQuery(entityId, resourceType.ToEntityType(), pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }

    [HttpPost(ApiEndpoints.Reactions.ReactOnEntity)]
    public async Task<IActionResult> ReactOnEntity(Guid entityId, string resourceType, CreateReactionRequest request)
    {
        if (!Enum.TryParse<ReactionType>(request.ReactionType, true, out var reactionType))
            return Problem(detail: $"ReactionType with value '{request.ReactionType}' is not found.");

        var reaction = new Reaction(ListenerId, entityId, resourceType.ToEntityType(), reactionType);
        var command = new ToggleReactionCommand(reaction);
        var toggleReactionResult = await _sender.Send(command);
        return toggleReactionResult.Match(Ok, Problem);
    }
}