namespace Audora.Contracts.Episodes.Responses;

public class EpisodesResponse
{
    public IEnumerable<EpisodeResponse> Episodes { get; init; } = null!;
}