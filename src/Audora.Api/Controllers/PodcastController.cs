using Audora.Application.Common.Models;
using Audora.Application.Podcasts.Queries.GetPodcastById;
using Audora.Application.Podcasts.Queries.ListCreatorPodcasts;
using Audora.Application.Podcasts.Queries.ListFollowedPodcasts;
using Audora.Application.Podcasts.Queries.ListPodcasts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class PodcastController : ApiController
{
    private readonly ISender _sender;

    public PodcastController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet(ApiEndpoints.Podcasts.List)]
    public async Task<IActionResult> ListPodcasts([FromQuery] Pagination pagination)
    {
        var query = new ListPodcastsQuery(ListenerId, pagination);
        var listPodcastsResult = await _sender.Send(query);
        return listPodcastsResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Podcasts.GetById)]
    public async Task<IActionResult> GetPodcastById(Guid podcastId)
    {
        var query = new GetPodcastByIdQuery(podcastId, ListenerId);
        var getPodcastResult = await _sender.Send(query);
        return getPodcastResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Creators.ListCreatorPodcasts)]
    public async Task<IActionResult> ListCreatorPodcasts(Guid creatorId, [FromQuery] Pagination pagination)
    {
        var query = new ListCreatorPodcastsQuery(creatorId, pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }
    
    [HttpGet(ApiEndpoints.Listeners.ListFollowedPodcasts)]
    public async Task<IActionResult> ListListenerFollowedPodcasts(Guid listenerId, [FromQuery] Pagination pagination)
    {
        var query = new ListFollowedPodcastsQuery(listenerId, pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }
}