using Audora.Application.Common;
using Audora.Application.Stats.Queries.GetEngagementStats;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class EngagementStatController : ApiController
{
    private readonly ISender _sender;

    public EngagementStatController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet(ApiEndpoints.EngagementStat.GetEngagementStats)]
    public async Task<IActionResult> GetEngagementStats(Guid entityId, string resourceType)
    {
        var query = new GetEngagementStatsQuery(entityId, resourceType.ToEntityType());
        var getEngagementStatResult = await _sender.Send(query);
        return getEngagementStatResult.Match(Ok, Problem);
    }
}