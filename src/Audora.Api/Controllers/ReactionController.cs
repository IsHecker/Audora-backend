using Audora.Application.Reactions.Queries.GetListenerReaction;
using Audora.Domain.Common.Enums;
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
    public async Task<IActionResult> GetListenerReactionForEntity(Guid listenerId, Guid entityId, [FromQuery] string entityType)
    {
        if (!Enum.TryParse<EntityType>(entityType, true, out var result))
            return Problem(detail: $"EntityType with value: '{entityType} is not found.'");

        var query = new GetListenerReactionQuery(listenerId, entityId, result);
        var listenerReactionResult = await _sender.Send(query);
        return listenerReactionResult.Match(Ok, Problem);
    }
}