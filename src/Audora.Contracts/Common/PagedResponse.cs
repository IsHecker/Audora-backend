namespace Audora.Contracts.Common;

public class PagedResponse<T>
{
    public IEnumerable<T> Items { get; init; } = null!;

    public int PageNumber { get; init; }

    public int PageSize { get; init; }

    // Total number of items across all pages
    public int TotalCount { get; init; }

    // Total number of pages calculated from TotalCount and PageSize
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

    // Indicates if there is a page before the current one
    public bool HasPreviousPage => PageNumber > 1;

    // Indicates if there is a page after the current one
    public bool HasNextPage => PageNumber < TotalPages;
}