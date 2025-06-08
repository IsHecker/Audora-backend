using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Playlists.Responses;

namespace Audora.Application.Playlists.Queries;

public record GetPlaylistByIdQuery(Guid PlaylistId) : ICommand<PlaylistResponse>;

public class GetPlaylistByIdQueryHandler : ICommandHandler<GetPlaylistByIdQuery, PlaylistResponse>
{
    private readonly IPlaylistRepository _playlistRepository;

    public GetPlaylistByIdQueryHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Result<PlaylistResponse>> Handle(GetPlaylistByIdQuery request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.GetByIdAsync(request.PlaylistId);

        if (playlist is null)
            return Error.NotFound($"Playlist with ID '{request.PlaylistId}' is not found.");

        return playlist.ToResponse();
    }
}