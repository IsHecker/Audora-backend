using Audora.Application.Analytics.Queries.GetEpisodeAnalytics;
using Audora.Application.Analytics.Queries.ListEpisodesAnalytics;
using Audora.Application.Common.Models;
using Audora.Application.Episodes.Queries.GetEpisodeById;
using Audora.Application.Episodes.Queries.ListEpisodes;
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

    [HttpGet(ApiEndpoints.Podcasts.ListPodcastEpisodes)]
    public async Task<IActionResult> ListPodcastEpisodes(Guid podcastId, bool details, [FromQuery] Pagination pagination)
    {
        var query = new ListEpisodesQuery(podcastId, "podcasts", ListenerId, pagination, details);
        var listPodcastsResult = await _sender.Send(query);
        return listPodcastsResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Episodes.GetById)]
    public async Task<IActionResult> GetEpisodeById(Guid episodeId)
    {
        var query = new GetEpisodeByIdQuery(ListenerId, episodeId);
        var getEpisodeResult = await _sender.Send(query);
        return getEpisodeResult.Match(Ok, Problem);
    }

    [HttpGet(ApiEndpoints.Playlists.ListPlaylistEpisodes)]
    public async Task<IActionResult> ListPlaylistEpisodes(Guid playlistId, bool details, [FromQuery] Pagination pagination)
    {
        var query = new ListEpisodesQuery(playlistId, "playlists", ListenerId, pagination, details);
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
}