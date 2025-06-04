using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.PlaybackSessions.Responses;

namespace Audora.Application.PlaybackSessions.Queries.ListPlaybackHistory;

public record ListPlaybackHistoryQuery(Guid ListenerId, Pagination Pagination) : IQuery<PagedResponse<PlaybackSessionResponse>>;

public class ListPlaybackHistoryQueryHandler : IQueryHandler<ListPlaybackHistoryQuery, PagedResponse<PlaybackSessionResponse>>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;
    private readonly IEpisodeRepository _episodeRepository;

    public ListPlaybackHistoryQueryHandler(
        IPlaybackSessionRepository playbackSessionRepository,
        IEpisodeRepository episodeRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
        _episodeRepository = episodeRepository;
    }

    public async Task<Result<PagedResponse<PlaybackSessionResponse>>> Handle(ListPlaybackHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var listenerSessions = await _playbackSessionRepository.GetAllByListenerId(request.ListenerId);

        var response = listenerSessions
            .Paginate(request.Pagination)
            .ToResponse().ToList();

        await AttachEpisodesToResponse(response);

        return response.ToPagedResponse(request.Pagination, listenerSessions.Count());
    }

    private async Task AttachEpisodesToResponse(List<PlaybackSessionResponse> response)
    {
        var episodeIds = response.Select(r => r.EpisodeId);

        var episodesResponse = (await _episodeRepository.GetAllAsync())
            .Where(ep => episodeIds.Contains(ep.Id))
            .Select(
                ep => new SmallEpisodeResponse
                {
                    Id = ep.Id,
                    Name = ep.Name,
                    CoverImageUrl = ep.CoverImageUrl,
                    AudioFileId = ep.AudioFileId,
                    Duration = ep.Duration,
                    PodcastName = ep.PodcastName
                })
            .ToDictionary(ep => ep.Id);

        foreach (var item in response)
        {
            item.Episode = episodesResponse[item.EpisodeId];
        }
    }
}