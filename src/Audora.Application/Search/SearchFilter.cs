using Audora.Application.Common.Models;

namespace Audora.Application.Search;

public class SearchFilter
{
    public string? Name { get; init; }
    public string? Category { get; init; }
    public string[]? Tags { get; init; }
    public string? Creator { get; init; }
    public float? Rating { get; init; }
    public string? Language { get; init; }
    public string? SortField { get; init; }
    public SortOrder? SortOrder { get; init; }
    public Pagination Pagination { get; init; } = null!;
}