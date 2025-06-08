using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Contracts.Playlists.Requests;

namespace Audora.Application.Playlists.Commands;

public record UpdatePlaylistEpisodesCommand(Guid PlaylistId, UpdatePlaylistEpisodesRequest UpdatePlaylistEpisodes) : ICommand;

public class UpdatePlaylistEpisodesCommandHandler : ICommandHandler<UpdatePlaylistEpisodesCommand>
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IPlaylistEpisodeRepository _playlistEpisodeRepository;

    public UpdatePlaylistEpisodesCommandHandler(
        IPlaylistRepository playlistRepository,
        IPlaylistEpisodeRepository playlistEpisodeRepository)
    {
        _playlistRepository = playlistRepository;
        _playlistEpisodeRepository = playlistEpisodeRepository;
    }

    public async Task<Result> Handle(UpdatePlaylistEpisodesCommand request, CancellationToken cancellationToken)
    {
        var playlistId = request.PlaylistId;

        if (request.UpdatePlaylistEpisodes.Added is List<Guid> addedEpisodes && addedEpisodes.Count > 0)
        {
            var playlist = await _playlistRepository.AsTracking().GetByIdAsync(playlistId);
            if (playlist is null)
                return Error.NotFound($"Playlist with ID '{playlistId}' is not found.");

            playlist.AddEpisodes(addedEpisodes);
        }

        if (request.UpdatePlaylistEpisodes.Removed is List<Guid> removedEpisodes && removedEpisodes.Count > 0)
        {
            var isDeleted = await _playlistEpisodeRepository.DeleteAsync(playlistId, removedEpisodes);
            if (!isDeleted)
                return Error.NotFound($"These IDs [{string.Join(", ", $"'{playlistId}'")}] is not found.");
        }

        return Result.Success;
    }
}