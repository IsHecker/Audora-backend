using Audora.Application.Common.Models;
using Audora.Application.Follows.Commands.ToggleFollow;
using Audora.Application.Follows.Queries.ListPodcastFollowers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class FollowController : ApiController
{
    private readonly ISender _sender;

    public FollowController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet(ApiEndpoints.Podcasts.ListFollowers)]
    public async Task<IActionResult> ListPodcastFollowers(Guid podcastId, [FromQuery] Pagination pagination)
    {
        var query = new ListPodcastFollowersQuery(podcastId, pagination);
        var listFollowersResult = await _sender.Send(query);
        return listFollowersResult.Match(Ok, Problem);
    }

    [HttpPost(ApiEndpoints.Follows.FollowEntity)]
    public async Task<IActionResult> FollowEntity(Guid entityId, string resourceType)
    {
        var command = new TogglePodcastFollowCommand(ListenerId, entityId);
        var followingResult = await _sender.Send(command);
        return followingResult.Match(NoContent, Problem);
    }
}