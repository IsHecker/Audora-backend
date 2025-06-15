using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Common;
using Audora.Contracts.Playlists.Responses;

namespace Audora.Application.Playlists.Queries;

public record ListListenerPlaylistsQuery(Guid ListenerId, Pagination Pagination) : ICommand<PagedResponse<PlaylistResponse>>;

public class ListListenerPlaylistsQueryHandler : ICommandHandler<ListListenerPlaylistsQuery, PagedResponse<PlaylistResponse>>
{
    private readonly IPlaylistRepository _playlistRepository;

    public ListListenerPlaylistsQueryHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Result<PagedResponse<PlaylistResponse>>> Handle(ListListenerPlaylistsQuery request, CancellationToken cancellationToken)
    {
        var playlists = await _playlistRepository.GetAllByListenerIdAsync(request.ListenerId);

        return playlists
            .Paginate(request.Pagination)
            .ToResponse()
            .ToPagedResponse(request.Pagination, playlists.Count());
    }
}