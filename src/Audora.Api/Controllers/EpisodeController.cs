using Audora.Application.Analytics.Queries.GetEpisodeAnalytics;
using Audora.Application.Analytics.Queries.ListEpisodesAnalytics;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Episodes.Commands.CreateEpisode;
using Audora.Application.Episodes.Commands.DeleteEpisode;
using Audora.Application.Episodes.Commands.UpdateEpisode;
using Audora.Application.Episodes.Queries.GetEpisodeById;
using Audora.Application.Episodes.Queries.ListEpisodesByParentId;
using Audora.Contracts.Episodes.Requests;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class EpisodeController : ApiController
{
    private readonly ISender _sender;

    public EpisodeController(ISender sender)
    {
        _sender = sender;
    }


    [HttpGet(ApiEndpoints.Episodes.GetById)]
    public async Task<IActionResult> GetEpisodeById(Guid episodeId)
    {
        var query = new GetEpisodeByIdQuery(ListenerId, episodeId);
        var getEpisodeResult = await _sender.Send(query);
        return getEpisodeResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Podcasts.ListPodcastEpisodes)]
    public async Task<IActionResult> ListPodcastEpisodes(Guid podcastId, bool details, [FromQuery] Pagination pagination)
    {
        var query = new ListEpisodesByParentIdQuery(podcastId, "podcasts", ListenerId, pagination, details);
        var listPodcastsResult = await _sender.Send(query);
        return listPodcastsResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Playlists.ListPlaylistEpisodes)]
    public async Task<IActionResult> ListPlaylistEpisodes(Guid playlistId, bool details, [FromQuery] Pagination pagination)
    {
        var query = new ListEpisodesByParentIdQuery(playlistId, "playlists", ListenerId, pagination, details);
        var listPlaylistsResult = await _sender.Send(query);
        return listPlaylistsResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Episodes.GetStats)]
    public async Task<IActionResult> GetEpisodeStats(Guid episodeId)
    {
        var query = new GetEpisodeAnalyticsQuery(episodeId);
        var getEpisodeAnalyticsResult = await _sender.Send(query);
        return getEpisodeAnalyticsResult.Match(Ok, Problem);
    }


    [HttpPost(ApiEndpoints.Podcasts.CreateEpisode)]
    public async Task<IActionResult> CreateEpisodeForPodcast(Guid podcastId, CreateEpisodeRequest request)
    {
        var command = new CreateEpisodeCommand(podcastId, request.ToDomain());
        var createEpisodeResult = await _sender.Send(command);
        return createEpisodeResult.Match(
            episode => CreatedAtAction(
                nameof(GetEpisodeById),
                new { episodeId = episode.Id },
                episode.ToResponse()
            ),
            Problem
        );
    }

    [HttpPut(ApiEndpoints.Episodes.Update)]
    public async Task<IActionResult> UpdateEpisode(Guid episodeId, UpdateEpisodeRequest request)
    {
        var command = new UpdateEpisodeCommand(episodeId, request.ToDomain());
        var updateEpisodeResult = await _sender.Send(command);
        return updateEpisodeResult.Match(NoContent, Problem);
    }

    [HttpDelete(ApiEndpoints.Episodes.Delete)]
    public async Task<IActionResult> DeleteEpisode(Guid episodeId)
    {
        var command = new DeleteEpisodeCommand(episodeId);
        var deleteEpisodeResult = await _sender.Send(command);
        return deleteEpisodeResult.Match(NoContent, Problem);
    }
}