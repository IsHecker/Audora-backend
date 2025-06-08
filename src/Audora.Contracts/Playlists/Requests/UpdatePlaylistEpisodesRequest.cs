namespace Audora.Contracts.Playlists.Requests;

public class UpdatePlaylistEpisodesRequest
{
    public IEnumerable<Guid>? Added { get; init; }
    public IEnumerable<Guid>? Removed { get; init; }

    // TODO: manage ordering problem later.
    public IEnumerable<PlaylistEpisodeReorderRequest>? Reordered { get; init; }
}

public class PlaylistEpisodeRequest
{
    public Guid EpisodeId { get; init; }
    public int Order { get; init; }
}

public class PlaylistEpisodeReorderRequest
{
    public PlaylistEpisodeRequest OldOrder { get; init; } = null!;
    public PlaylistEpisodeRequest NewOrder { get; init; } = null!;
}