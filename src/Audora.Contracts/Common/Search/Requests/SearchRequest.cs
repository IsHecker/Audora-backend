namespace Audora.Contracts.Common.Search.Requests;

public class SearchRequest
{
    public string? Keyword { get; set; }
    public string? Category { get; set; }
    public string[]? Tags { get; set; }
    public string? Creator { get; set; }
    public float? Rating { get; set; }
    public string? Language { get; set; }
    public string? SortBy { get; set; }
}