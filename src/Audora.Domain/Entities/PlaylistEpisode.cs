namespace Audora.Domain.Entities;

public class PlaylistEpisode
{
    public Guid PlaylistId { get; init; }
    public Playlist Playlist { get; set; } = null!;

    public Guid EpisodeId { get; init; }
    public Episode Episode { get; set; } = null!;

    public int Order { get; init; }
    public DateTime AddedAt { get; init; }
}