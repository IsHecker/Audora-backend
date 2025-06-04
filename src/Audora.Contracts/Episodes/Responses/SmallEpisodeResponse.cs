namespace Audora.Contracts.Episodes.Responses;

public class SmallEpisodeResponse
{
    public Guid Id { get; init; }
    public string PodcastName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public long Duration { get; init; }
    public Guid AudioFileId { get; init; }
}