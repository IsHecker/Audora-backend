using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Common;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Application.Podcasts.Queries.ListPodcasts;

public record ListPodcastsQuery(Guid ListenerId, Pagination Pagination) : IQuery<PagedResponse<PodcastResponse>>;

public class ListPodcastsQueryHandler : IQueryHandler<ListPodcastsQuery, PagedResponse<PodcastResponse>>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly PodcastResponseAttacher _podcastResponseAttacher;

    public ListPodcastsQueryHandler(IPodcastRepository podcastRepository,
        PodcastResponseAttacher podcastResponseAttacher)
    {
        _podcastRepository = podcastRepository;
        _podcastResponseAttacher = podcastResponseAttacher;
    }

    public async Task<Result<PagedResponse<PodcastResponse>>> Handle(ListPodcastsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO Get trending/featured/new podcasts
        // filter parameter (e.g., type = "trending" | "new" | "featured").

        var podcasts = await _podcastRepository.GetAllAsync();

        var response = podcasts.Paginate(request.Pagination).ToResponse().ToList();

        return _podcastResponseAttacher.AttachTo(response)
            .AttachFollowStatus(request.ListenerId)
            .AttachRatings(request.ListenerId)
            .GetResponseCollection()
            .ToPagedResponse(request.Pagination, podcasts.Count());
    }
}