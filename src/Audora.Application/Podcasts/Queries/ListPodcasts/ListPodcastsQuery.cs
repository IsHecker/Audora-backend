using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Podcasts.Queries.ListPodcasts;

public record ListPodcastsQuery(Guid ListenerId, Pagination Pagination) : IQuery<PodcastsResponse>;

public class ListPodcastsQueryHandler : IQueryHandler<ListPodcastsQuery, PodcastsResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly PodcastViewService _podcastViewService;

    public ListPodcastsQueryHandler(IPodcastRepository podcastRepository, PodcastViewService podcastViewService)
    {
        _podcastRepository = podcastRepository;
        _podcastViewService = podcastViewService;
    }

    public async Task<Result<PodcastsResponse>> Handle(ListPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO Get trending/featured/new podcasts
        // filter parameter (e.g., type = "trending" | "new" | "featured").

        var podcasts = (await _podcastRepository.GetAllAsync()).Paginate(request.Pagination);

        return await _podcastViewService.AttachListenerMetadataAsync(podcasts, request.ListenerId);
    }
}