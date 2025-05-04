using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Analytics.Queries.GetEpisodeAnalytics;

public record GetEpisodeAnalyticsQuery(Guid PodcastStatId, Pagination Pagination) : IQuery<IEnumerable<EpisodeStat>>;

public class GetEpisodeAnalyticsQueryHandler : IQueryHandler<GetEpisodeAnalyticsQuery, IEnumerable<EpisodeStat>>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;

    public GetEpisodeAnalyticsQueryHandler(IEpisodeStatRepository episodeStatRepository)
    {
        _episodeStatRepository = episodeStatRepository;
    }

    public async Task<Result<IEnumerable<EpisodeStat>>> Handle(GetEpisodeAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO return error when PodcastStat id doesn't exist.

        return (await _episodeStatRepository.GetEpisodeStatsByPodcastStateId(request.PodcastStatId))
            .Paginate(request.Pagination).ToResult<IEnumerable<EpisodeStat>>();
    }
}