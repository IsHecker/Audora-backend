namespace Audora.Contracts.Episodes.Requests;

public class UpdateEpisodeRequest
{
    public Guid AudioFileId { get; init; }
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; init; }
}