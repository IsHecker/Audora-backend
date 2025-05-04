namespace Audora.Contracts.Podcasts.Requests;

public class UpdatePodcastRequest
{
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; init; }
    public string Category { get; init; } = null!;
    public string Language { get; init; } = null!;
    public string[]? Tags { get; init; } = null!;
}