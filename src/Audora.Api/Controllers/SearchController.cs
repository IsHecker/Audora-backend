using Audora.Application.Common.Models;
using Audora.Application.Follows.Queries.ListPodcastFollowers;
using Audora.Application.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class SearchController : ApiController
{
    private readonly ISender _sender;

    public SearchController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet(ApiEndpoints.Search.GlobalSearch)]
    public async Task<IActionResult> GlobalSearch([FromQuery] SearchFilter searchFilter, [FromQuery] Pagination pagination)
    {
        var query = new SearchQuery(searchFilter, pagination);
        var searchResult = await _sender.Send(query);
        return searchResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Search.MixedSearch)]
    public async Task<IActionResult> MixedSearch([FromQuery] SearchFilter searchFilter, [FromQuery] Pagination pagination)
    {
        var query = new SearchQuery(searchFilter, pagination, IsMixed: true);
        var searchResult = await _sender.Send(query);
        return searchResult.Match(Ok, Problem);
    }
}