using Audora.Application.Common;
using Audora.Application.Common.Models;
using Audora.Application.PlaybackSessions.Commands.GetOrCreatePlaybackSession;
using Audora.Application.PlaybackSessions.Commands.MarkPlaybackSessionProgress;
using Audora.Application.PlaybackSessions.Queries.ListPlaybackHistory;
using Audora.Contracts.PlaybackSessions.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class PlaybackSessionController : ApiController
{
    private readonly ISender _sender;

    public PlaybackSessionController(ISender sender)
    {
        _sender = sender;
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpGet(ApiEndpoints.PlaybackSessions.ListPlaybackSessionHistory)]
    public async Task<IActionResult> ListPlaybackSessionHistory([FromQuery] Pagination pagination)
    {
        var query = new ListPlaybackHistoryQuery(ListenerId!.Value, pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpPost(ApiEndpoints.PlaybackSessions.GetOrCreatePlaybackSession)]
    public async Task<IActionResult> GetOrCreatePlaybackSession(Guid episodeId)
    {
        var query = new GetOrCreatePlaybackSessionCommand(ListenerId!.Value, episodeId);
        var getOrCreateResult = await _sender.Send(query);
        return getOrCreateResult.Match(Ok, Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpPatch(ApiEndpoints.PlaybackSessions.MarkPlaybackProgress)]
    public async Task<IActionResult> MarkPlaybackProgress(Guid sessionId, MarkSessionProgressRequest request)
    {
        var command = new MarkPlaybackSessionProgressCommand(sessionId, request);
        var markSessionResult = await _sender.Send(command);
        return markSessionResult.Match(NoContent, Problem);
    }
}