namespace Audora.Contracts.Episodes.Responses;

public class EpisodesSummaryResponse
{
    public IEnumerable<EpisodeSummaryResponse> Episodes { get; init; } = null!;
}

public class EpisodeSummaryResponse
{
    public string Name { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public long Duration { get; init; }
    public Guid AudioFileId { get; init; }
    public string PodcastName { get; init; } = null!;
    public int EpisodeNumber { get; init; }
    public int PlayCount { get; init; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTime ReleaseDate { get; init; }
}