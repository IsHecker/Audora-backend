using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Analytics.Responses;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Analytics.Queries.GetPodcastAnalytics;

public record GetPodcastAnalyticsQuery(Guid PodcastId) : IQuery<PodcastAnalyticsResponse>;

public class GetPodcastAnalyticsQueryHandler : IQueryHandler<GetPodcastAnalyticsQuery, PodcastAnalyticsResponse>
{
    private readonly IPodcastStatRepository _podcastStatRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;

    public GetPodcastAnalyticsQueryHandler(IPodcastStatRepository podcastStatRepository,
        IEpisodeStatRepository episodeStatRepository)
    {
        _podcastStatRepository = podcastStatRepository;
        _episodeStatRepository = episodeStatRepository;
    }

    public async Task<Result<PodcastAnalyticsResponse>> Handle(GetPodcastAnalyticsQuery request,
        CancellationToken cancellationToken)
    {
        // TODO return error when podcast id doesn't exist.
        // TODO maybe combine it with episode stats.

        var podcastStat = await _podcastStatRepository
            .IncludePodcast()
            .GetByPodcastIdAsync(request.PodcastId);

        var episodeStats = await _episodeStatRepository.GetAllByPodcastStateId(request.PodcastId);

        return podcastStat.ToResponse(episodeStats);
    }
}