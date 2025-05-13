using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Analytics.Responses;

namespace Audora.Application.Analytics.Queries.GetEpisodeAnalytics;

public record GetEpisodeAnalyticsQuery(Guid EpisodeId) : IQuery<EpisodeAnalyticsResponse>;

public class GetEpisodeAnalyticsQueryHandler : IQueryHandler<GetEpisodeAnalyticsQuery, EpisodeAnalyticsResponse>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public GetEpisodeAnalyticsQueryHandler(IEpisodeStatRepository episodeStatRepository,
        IEngagementStatRepository engagementStatRepository)
    {
        _episodeStatRepository = episodeStatRepository;
        _engagementStatRepository = engagementStatRepository;
    }

    public async Task<Result<EpisodeAnalyticsResponse>> Handle(GetEpisodeAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO return error when PodcastStat id doesn't exist.

        var episodeStat = await _episodeStatRepository.GetByEpisodeIdAsync(request.EpisodeId);
        var engagementsStat = await _engagementStatRepository.GetByEntityIdAsync(request.EpisodeId);
        return episodeStat.ToResponse(engagementsStat);
    }
}