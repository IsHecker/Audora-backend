using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Queries.GetPodcast;

public record GetPodcastQuery(Guid PodcastId, bool IncludeEpisodes) : IQuery<Podcast>;

public class GetPodcastQueryHandler : IQueryHandler<GetPodcastQuery, Podcast>
{
    private readonly IPodcastRepository _podcastRepository;

    public GetPodcastQueryHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result<Podcast>> Handle(GetPodcastQuery request, CancellationToken cancellationToken)
    {
        var podcast = await _podcastRepository
            .IncludeEpisodes(request.IncludeEpisodes)
            .GetByIdAsync(request.PodcastId);

        if (podcast is null)
        {
            return Error.NotFound(description: $"Podcast with Id '{request.PodcastId}' is not found.");
        }

        return podcast;
    }
}