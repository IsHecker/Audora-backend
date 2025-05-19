using Audora.Application.Common.Models;
using Audora.Contracts.Common;

namespace Audora.Application.Common.Mappings;

public static class PagingMappings
{
    public static PagedResponse<T> ToPagedResponse<T>(this IEnumerable<T> source,
        Pagination pagination,
        int totalCount)
    {
        return new PagedResponse<T>
        {
            Items = source,
            PageNumber = pagination.PageNumber,
            PageSize = pagination.PageSize,
            TotalCount = totalCount
        };
    }
}