using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;

namespace Audora.Application.Playlists.Commands;

public record DeletePlaylistCommand(Guid PlaylistId) : ICommand;

public class DeletePlaylistCommandHandler : ICommandHandler<DeletePlaylistCommand>
{
    private readonly IPlaylistRepository _playlistRepository;

    public DeletePlaylistCommandHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Result> Handle(DeletePlaylistCommand request, CancellationToken cancellationToken)
    {
        var isDeleted = await _playlistRepository.DeleteAsync(request.PlaylistId);

        if (!isDeleted)
            return Error.NotFound($"Playlist with ID '{request.PlaylistId}' is not found.");

        return Result.Success;
    }
}