using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Podcasts.Queries.ListCreatorPodcasts;

public record ListCreatorPodcastsQuery(Guid CreatorId, bool IncludeEpisodes, Pagination Pagination)
    : IQuery<PodcastsResponse>;

public class ListCreatorPodcastsQueryHandler : IQueryHandler<ListCreatorPodcastsQuery, PodcastsResponse>
{
    private readonly IPodcastRepository _podcastRepository;

    public ListCreatorPodcastsQueryHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result<PodcastsResponse>> Handle(ListCreatorPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var podcasts = await _podcastRepository
            .IncludeEpisodes(request.IncludeEpisodes)
            .GetAllAsync();

        return podcasts.Where(podcast => podcast.CreatorId == request.CreatorId)
            .Paginate(request.Pagination)
            .ToResponse();
    }
}