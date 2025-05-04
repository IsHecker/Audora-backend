namespace Audora.Contracts.Episodes.Requests;

public class CreateEpisodeRequest
{
    public Guid PodcastId { get; init; }
    public Guid AudioFileId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public string PodcastName { get; init; } = null!;
    public bool IsPublished { get; init; }
    public int EpisodeNumber { get; init; }
}