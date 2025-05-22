using Audora.Application.Common.Models;
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

}