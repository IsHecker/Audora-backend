using System.Text.Json.Serialization;
using Audora.Contracts.Common;
using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.Podcasts.Responses;

namespace Audora.Contracts.Search;

public class SearchResponse
{
    public IEnumerable<PodcastResponse>? Podcasts { get; init; } = null!;
    public IEnumerable<EpisodeResponse>? Episodes { get; init; } = null!;

    [JsonPropertyName("results")]
    public PagedResponse<SearchResultItem>? MixedResults { get; init; } = null!;
}

public class SearchResultItem
{
    public string Type { get; set; } = default!;
    public object Data { get; set; } = default!;
}