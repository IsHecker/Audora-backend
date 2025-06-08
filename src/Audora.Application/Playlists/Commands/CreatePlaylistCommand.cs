using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Playlists.Commands;

public record CreatePlaylistCommand(Playlist Playlist) : ICommand<Guid>;

public class CreatePlaylistCommandHandler : ICommandHandler<CreatePlaylistCommand, Guid>
{
    private readonly IPlaylistRepository _playlistRepository;

    public CreatePlaylistCommandHandler(IPlaylistRepository playlistRepository)
    {
        _playlistRepository = playlistRepository;
    }

    public async Task<Result<Guid>> Handle(CreatePlaylistCommand request, CancellationToken cancellationToken)
    {
        var playlist = await _playlistRepository.AddAsync(request.Playlist);
        return playlist.Id;
    }
}