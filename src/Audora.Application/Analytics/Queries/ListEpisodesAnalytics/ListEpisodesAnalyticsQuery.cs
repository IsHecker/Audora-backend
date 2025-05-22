using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Analytics.Responses;
using Audora.Contracts.Common;
using Audora.Domain.Entities;

namespace Audora.Application.Analytics.Queries.ListEpisodesAnalytics;

public record ListEpisodesAnalyticsQuery(Guid PodcastId, Pagination Pagination)
    : IQuery<PagedResponse<EpisodeAnalyticsResponse>>;

public class
    ListEpisodesAnalyticsQueryHandler : IQueryHandler<ListEpisodesAnalyticsQuery,
    PagedResponse<EpisodeAnalyticsResponse>>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public ListEpisodesAnalyticsQueryHandler(IEpisodeStatRepository episodeStatRepository,
        IEngagementStatRepository engagementStatRepository)
    {
        _episodeStatRepository = episodeStatRepository;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result<PagedResponse<EpisodeAnalyticsResponse>>> Handle(ListEpisodesAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO return error when PodcastStat id doesn't exist.

        var pagination = request.Pagination;

        var episodesStats = (await _episodeStatRepository.GetAllByPodcastId(request.PodcastId))
            .Paginate(pagination);

        return episodesStats.Select(es => es.ToResponse(GetEngagements(es.EpisodeId)))
            .ToPagedResponse(pagination, episodesStats.Count());
    }

    private EngagementStat GetEngagements(Guid episodeId)
    {
        return _engagementStatRepository.GetByEntityIdAsync(episodeId).GetAwaiter().GetResult()!;
    }
}