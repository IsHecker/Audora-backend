using Audora.Contracts.Episodes;
using Audora.Contracts.Episodes.Requests;

namespace Audora.Contracts.Podcasts.Requests;

public class CreatePodcastRequest
{
    public Guid CreatorId { get; init; }
    public string CreatorName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; init; }
    public string Category { get; init; } = null!;
    public string Language { get; init; } = null!;
    public string[]? Tags { get; init; } = null!;
    public CreateEpisodeRequest[]? Episodes { get; init; } = null!;
}