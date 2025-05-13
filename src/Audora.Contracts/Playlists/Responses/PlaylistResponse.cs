namespace Audora.Contracts.Playlists.Responses;

public class PlaylistResponse
{
    public Guid ListenerId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
}