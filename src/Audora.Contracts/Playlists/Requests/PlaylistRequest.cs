namespace Audora.Contracts.Playlists.Requests;

public class PlaylistRequest
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; } = null!;
    public bool IsPublic { get; init; }
}