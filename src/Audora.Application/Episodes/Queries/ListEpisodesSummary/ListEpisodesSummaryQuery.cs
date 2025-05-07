using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Episodes.Responses;

namespace Audora.Application.Episodes.Queries.ListEpisodesSummary;

public record ListEpisodesSummaryQuery(Guid PodcastId, Pagination Pagination) : IQuery<EpisodesSummaryResponse>;

public class ListEpisodesSummaryQueryHandler : IQueryHandler<ListEpisodesSummaryQuery, EpisodesSummaryResponse>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;

    public ListEpisodesSummaryQueryHandler(IEpisodeRepository episodeRepository,
        IEpisodeStatRepository episodeStatRepository)
    {
        _episodeRepository = episodeRepository;
        _episodeStatRepository = episodeStatRepository;
    }

    public async Task<Result<EpisodesSummaryResponse>> Handle(ListEpisodesSummaryQuery request,
        CancellationToken cancellationToken)
    {
        // TODO should check if the podcast id exists.

        var episodes = (await _episodeRepository.GetAllByPodcastIdAsync(request.PodcastId))
            .Paginate(request.Pagination);

        var episodeStatsDict = 
            (await _episodeStatRepository.GetAllByEpisodeIdsAsync(episodes.Select(ep => ep.Id)))
            .ToDictionary(stat => stat.Id, stat => stat);

        return episodes.ToResponse(episodeStatsDict);
    }
}