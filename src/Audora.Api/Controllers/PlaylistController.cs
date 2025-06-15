using Audora.Application.Common;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Playlists.Commands;
using Audora.Application.Playlists.Queries;
using Audora.Contracts.Playlists.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class PlaylistController : ApiController
{
    private readonly ISender _sender;

    public PlaylistController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet(ApiEndpoints.Playlists.GetById)]
    public async Task<IActionResult> GetPlaylistById(Guid playlistId)
    {
        var query = new GetPlaylistByIdQuery(playlistId);
        var getResult = await _sender.Send(query);
        return getResult.Match(Ok, Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpGet(ApiEndpoints.Playlists.ListMyPlaylists)]
    public async Task<IActionResult> ListMyPlaylists([FromQuery] Pagination pagination)
    {
        var query = new ListListenerPlaylistsQuery(ListenerId!.Value, pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }

    [Authorize]
    [HttpGet(ApiEndpoints.Listeners.ListPlaylistsByListener)]
    public async Task<IActionResult> ListPlaylistsByListener(Guid listenerId, [FromQuery] Pagination pagination)
    {
        var query = new ListListenerPlaylistsQuery(listenerId, pagination);
        var listResult = await _sender.Send(query);
        return listResult.Match(Ok, Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpPatch(ApiEndpoints.Playlists.UpdatePlaylistEpisodes)]
    public async Task<IActionResult> UpdatePlaylistEpisodes(Guid playlistId, UpdatePlaylistEpisodesRequest request)
    {
        var command = new UpdatePlaylistEpisodesCommand(playlistId, request);
        var updateResult = await _sender.Send(command);
        return updateResult.Match(NoContent, Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpPost(ApiEndpoints.Playlists.Create)]
    public async Task<IActionResult> CreatePlaylist(PlaylistRequest request)
    {
        var command = new CreatePlaylistCommand(request.ToDomain(ListenerId!.Value));
        var createResult = await _sender.Send(command);
        return createResult.Match(
            val => CreatedAtAction(nameof(GetPlaylistById), new { playlistId = val }, val),
            Problem);
    }

    [Authorize(Roles = Roles.Listener)]
    [HttpDelete(ApiEndpoints.Playlists.Delete)]
    public async Task<IActionResult> DeletePlaylist(Guid playlistId)
    {
        var command = new DeletePlaylistCommand(playlistId);
        var deleteResult = await _sender.Send(command);
        return deleteResult.Match(NoContent, Problem);
    }
}