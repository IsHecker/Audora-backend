using Audora.Application.Common.Models;
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
        var query = new ListPodcastsQuery(Guid.Parse("735331aa-72c7-4d48-a092-a0ce72a6c49e"), pagination);
        var listPodcastsResult = await _sender.Send(query);
        return listPodcastsResult.Match(Ok, Problem);
    }
}