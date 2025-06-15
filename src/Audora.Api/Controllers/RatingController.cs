using Audora.Application.Common;
using Audora.Application.Ratings.Commands.RatePodcast;
using Audora.Contracts.Podcasts.Requests;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Audora.Api.Controllers;

public class RatingController : ApiController
{
    private readonly ISender _sender;

    public RatingController(ISender sender)
    {
        _sender = sender;
    }


    [Authorize(Roles = Roles.Listener)]
    [HttpPost(ApiEndpoints.Ratings.RatePodcast)]
    public async Task<IActionResult> RatePodcast(Guid podcastId, RatePodcastRequest request)
    {
        var command = new RatePodcastCommand(podcastId, ListenerId!.Value, request.Rating);
        var ratingResult = await _sender.Send(command);
        return ratingResult.Match(Ok, Problem);
    }
}