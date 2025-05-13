using Audora.Contracts.Common;

namespace Audora.Application.Common.Mappings;

public static class PagingMappings
{
    public static PagedResponse<T> ToPagedResponse<T>(this IEnumerable<T> source,
        int pageNumber,
        int pageSize,
        int totalCount)
    {
        return new PagedResponse<T>
        {
            Items = source, PageNumber = pageNumber, PageSize = pageSize, TotalCount = totalCount
        };
    }
}