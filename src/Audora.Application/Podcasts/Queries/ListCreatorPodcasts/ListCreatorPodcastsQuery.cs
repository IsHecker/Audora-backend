using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Podcasts.Queries.ListCreatorPodcasts;

public record ListCreatorPodcastsQuery(Guid CreatorId, Pagination Pagination, bool IncludeEpisodes)
    : IQuery<IEnumerable<Podcast>>;

public class ListCreatorPodcastsQueryHandler : IQueryHandler<ListCreatorPodcastsQuery, IEnumerable<Podcast>>
{
    private readonly IPodcastRepository _podcastRepository;

    public ListCreatorPodcastsQueryHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result<IEnumerable<Podcast>>> Handle(ListCreatorPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var podcasts = await _podcastRepository
            .IncludeEpisodes(request.IncludeEpisodes)
            .GetAllAsync();

        return podcasts.Where(podcast => podcast.CreatorId == request.CreatorId)
            .Paginate(request.Pagination)
            .ToResult<IEnumerable<Podcast>>();
    }
}