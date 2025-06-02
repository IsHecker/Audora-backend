namespace Audora.Contracts.Podcasts.Responses;

public class SmallPodcastResponse
{
    public Guid Id { get; init; }
    public Guid CreatorId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public string Category { get; init; } = null!;
    public string Language { get; init; } = null!;
    public int TotalEpisodes { get; init; }
}