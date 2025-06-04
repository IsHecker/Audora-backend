namespace Audora.Contracts.Episodes.Requests;

public class CreateEpisodesRequest
{
    public IEnumerable<EpisodeDetails> Episodes { get; init; } = null!;
}

public class EpisodeDetails
{
    public Guid AudioFileId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; init; }
    public int EpisodeNumber { get; init; }
}
