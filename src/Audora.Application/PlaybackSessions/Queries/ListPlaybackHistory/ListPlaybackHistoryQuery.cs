using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.PlaybackSessions.Responses;

namespace Audora.Application.PlaybackSessions.Queries.ListPlaybackHistory;

public record ListPlaybackHistoryQuery(Guid ListenerId, Pagination Pagination) : IQuery<PagedResponse<PlaybackSessionResponse>>;

public class ListPlaybackHistoryQueryHandler : IQueryHandler<ListPlaybackHistoryQuery, PagedResponse<PlaybackSessionResponse>>
{
    private readonly IPlaybackSessionRepository _playbackSessionRepository;

    public ListPlaybackHistoryQueryHandler(IPlaybackSessionRepository playbackSessionRepository)
    {
        _playbackSessionRepository = playbackSessionRepository;
    }

    public async Task<Result<PagedResponse<PlaybackSessionResponse>>> Handle(ListPlaybackHistoryQuery request,
        CancellationToken cancellationToken)
    {
        var listenerSessions = await _playbackSessionRepository.GetAllByListenerId(request.ListenerId);

        return listenerSessions
            .Paginate(request.Pagination)
            .ToResponse()
            .ToPagedResponse(request.Pagination, listenerSessions.Count());
    }
}