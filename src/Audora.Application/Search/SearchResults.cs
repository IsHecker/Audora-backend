using System.Text.Json.Serialization;
using Audora.Contracts.Common;
using Audora.Domain.Entities;

namespace Audora.Application.Search;

public class SearchResults
{
    public IEnumerable<Podcast> Podcasts { get; init; } = null!;
    public IEnumerable<Episode> Episodes { get; init; } = null!;

    [JsonPropertyName("results")]
    // public PagedResponse<SearchResultItem>? MixedResults = null!;
    public PagedResponse<SearchResultItem>? MixedResults { get; init; } = null!;
}

public class SearchResultItem
{
    public string Type { get; set; } = default!;
    public object Data { get; set; } = default!;
}