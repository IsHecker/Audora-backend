using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Search;
using Audora.Contracts.Search.Requests;
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
    public async Task<IActionResult> GlobalSearch([FromQuery] SearchRequest searchRequest, [FromQuery] Pagination pagination)
    {
        var query = new SearchQuery(searchRequest.ToFilter(), pagination, IsMixed: false);
        var searchResult = await _sender.Send(query);
        return searchResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Search.MixedSearch)]
    public async Task<IActionResult> MixedSearch([FromQuery] SearchRequest searchRequest, [FromQuery] Pagination pagination)
    {
        var query = new SearchQuery(searchRequest.ToFilter(), pagination, IsMixed: true);
        var searchResult = await _sender.Send(query);
        return searchResult.Match(Ok, Problem);
    }
}