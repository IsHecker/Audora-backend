using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Podcasts.Queries.ListCreatorPodcasts;

public record ListCreatorPodcastsQuery(Guid CreatorId, Pagination Pagination)
    : IQuery<PagedResponse<PodcastResponse>>;

public class ListCreatorPodcastsQueryHandler : IQueryHandler<ListCreatorPodcastsQuery, PagedResponse<PodcastResponse>>
{
    private readonly IPodcastRepository _podcastRepository;

    public ListCreatorPodcastsQueryHandler(IPodcastRepository podcastRepository)
    {
        _podcastRepository = podcastRepository;
    }

    public async Task<Result<PagedResponse<PodcastResponse>>> Handle(ListCreatorPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        var podcasts = await _podcastRepository.GetAllAsync();

        var creatorPodcasts = podcasts.Where(podcast => podcast.CreatorId == request.CreatorId);

        return creatorPodcasts
            .Paginate(request.Pagination)
            .ToResponse()
            .ToPagedResponse(request.Pagination, creatorPodcasts.Count());
    }
}