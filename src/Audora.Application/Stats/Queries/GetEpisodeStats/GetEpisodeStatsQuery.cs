using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Analytics.Responses;
using Audora.Domain.Common.Enums;

namespace Audora.Application.Stats.Queries.GetEpisodeStats;

public record GetEpisodeStatsQuery(Guid EpisodeId) : IQuery<EpisodeStatsResponse>;

public class GetEpisodeStatsQueryHandler : IQueryHandler<GetEpisodeStatsQuery, EpisodeStatsResponse>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly EngagementStatsService _engagementStatsService;

    public GetEpisodeStatsQueryHandler(
        IEpisodeStatRepository episodeStatRepository,
        EngagementStatsService engagementStatsService)
    {
        _episodeStatRepository = episodeStatRepository;
        _engagementStatsService = engagementStatsService;
    }

    public async Task<Result<EpisodeStatsResponse>> Handle(GetEpisodeStatsQuery request,
          CancellationToken cancellationToken)
    {
        // TODO return error when PodcastStat id doesn't exist.

        var episodeStat = await _episodeStatRepository.GetByEpisodeIdAsync(request.EpisodeId);
        var engagementsStatResult = await _engagementStatsService.GetStatsAsync(request.EpisodeId, EntityType.Episode);

        if (engagementsStatResult.IsError)
            return engagementsStatResult.Errors;

        return episodeStat.ToResponse(engagementsStatResult.Value);
    }
}