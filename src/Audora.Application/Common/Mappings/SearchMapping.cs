using Audora.Application.Common.Models;
using Audora.Application.Search;
using Audora.Contracts.Search.Requests;

namespace Audora.Application.Common.Mappings;

public static class SearchMapping
{
    public static SearchFilter ToFilter(this SearchRequest request)
    {
        return new SearchFilter
        {
            Name = request.Keyword,
            Creator = request.Keyword,
            Category = request.Category,
            Language = request.Language,
            Rating = request.Rating,
            Tags = request.Tags,
            SortField = request.SortBy?.TrimStart('-', '+'),
            SortOrder = request.SortBy is null ? SortOrder.Unsorted :
                request.SortBy.StartsWith('-') ?
                SortOrder.Descending
                : SortOrder.Ascending
        };
    }
}