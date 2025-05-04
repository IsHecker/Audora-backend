using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Contracts.Common.Search.Responses;

public class SearchResponse
{
    public IEnumerable<PodcastResponse>  Podcasts { get; init; } = null!;
    public IEnumerable<EpisodeResponse>  Episodes { get; init; } = null!;
}