using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Follows.Queries.ListPodcastFollowers;
using Audora.Application.Podcasts.Commands.CreatePodcast;
using Audora.Application.Podcasts.Commands.DeletePodcast;
using Audora.Application.Podcasts.Commands.UpdatePodcast;
using Audora.Application.Podcasts.Queries.GetPodcastById;
using Audora.Application.Podcasts.Queries.ListCreatorPodcasts;
using Audora.Application.Podcasts.Queries.ListFollowedPodcasts;
using Audora.Application.Podcasts.Queries.ListPodcasts;
using Audora.Contracts.Podcasts.Requests;
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


    [HttpPost(ApiEndpoints.Podcasts.Create)]
    public async Task<IActionResult> CreatePodcast(CreatePodcastRequest request)
    {
        var command = new CreatePodcastCommand(request.ToDomain(), request.Episodes?.ToDomain());
        var createResult = await _sender.Send(command);
        return createResult.Match(
            response => CreatedAtAction(
                nameof(GetPodcastById),
                new { podcastId = response.Id },
                response
            ),
            Problem
        );
    }

    [HttpPut(ApiEndpoints.Podcasts.Update)]
    public async Task<IActionResult> UpdatePodcast(Guid podcastId, UpdatePodcastRequest request)
    {
        var command = new UpdatePodcastCommand(podcastId, request.ToDomain());
        var updateResult = await _sender.Send(command);
        return updateResult.Match(NoContent, Problem);
    }

    [HttpDelete(ApiEndpoints.Podcasts.Delete)]
    public async Task<IActionResult> DeletePodcast(Guid podcastId)
    {
        var command = new DeletePodcastCommand(podcastId);
        var deleteResult = await _sender.Send(command);
        return deleteResult.Match(NoContent, Problem);
    }
}