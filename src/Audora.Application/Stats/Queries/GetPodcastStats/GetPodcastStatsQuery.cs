using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Analytics.Responses;

namespace Audora.Application.Stats.Queries.GetPodcastStats;

public record GetPodcastStatsQuery(Guid PodcastId) : IQuery<PodcastStatsResponse>;

public class GetPodcastStatsQueryHandler : IQueryHandler<GetPodcastStatsQuery, PodcastStatsResponse>
{
    private readonly IPodcastStatRepository _podcastStatRepository;

    public GetPodcastStatsQueryHandler(IPodcastStatRepository podcastStatRepository)
    {
        _podcastStatRepository = podcastStatRepository;
    }

    public async Task<Result<PodcastStatsResponse>> Handle(GetPodcastStatsQuery request,
        CancellationToken cancellationToken)
    {
        var podcastStat = await _podcastStatRepository.GetByPodcastIdAsync(request.PodcastId);

        if (podcastStat is null)
            return Error.NotFound("Podcast", $"Podcast with ID '{request.PodcastId}' was not found.");

        return podcastStat.ToResponse();
    }
}