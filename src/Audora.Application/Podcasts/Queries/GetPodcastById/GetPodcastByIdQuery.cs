using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Podcasts.Queries.GetPodcastById;

public record GetPodcastByIdQuery(Guid PodcastId, Guid? ListenerId) : IQuery<PodcastResponse>;

public class GetPodcastByIdQueryHandler : IQueryHandler<GetPodcastByIdQuery, PodcastResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly PodcastResponseAttacher _podcastResponseAttacher;

    public GetPodcastByIdQueryHandler(
        IPodcastRepository podcastRepository,
        PodcastResponseAttacher podcastResponseAttacher)
    {
        _podcastRepository = podcastRepository;
        _podcastResponseAttacher = podcastResponseAttacher;
    }

    public async Task<Result<PodcastResponse>> Handle(GetPodcastByIdQuery request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository
            .GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        if (!podcast.IsPublished)
        {
            return Error.Forbidden(description: $"This Podcast is Unpublished.");
        }

        // return podcast.ToResponse();
        var response = podcast.ToResponse();
        if (request.ListenerId is null)
            return response;

        return _podcastResponseAttacher.AttachTo(response)
                    .AttachFollowStatus(request.ListenerId!.Value)
                    .AttachRatings(request.ListenerId!.Value)
                    .GetSingleResponse();
    }
}