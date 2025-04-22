namespace Audora.Domain.Entities;

/// <summary>
/// Join Table used by EF Core.
/// </summary>
public class PlaylistEpisode
{
    public Guid PlaylistId { get; init; }
    public Guid EpisodeId { get; init; }
    public int Order { get; init; }
    public DateTime AddedAt { get; init; }
}