using Audora.Application.Common.Models;

namespace Audora.Application.Common;

public static class Extensions
{
    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, Pagination pagination)
    {
        return query.Skip(pagination.PageSize * (pagination.PageNumber - 1)).Take(pagination.PageSize);
    }

    public static IQueryable<T> Paginate<T>(this IQueryable<T> query, int pageNumber, int pageSize)
    {
        return query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
    }
}