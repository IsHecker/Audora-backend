using System.Linq.Expressions;
using System.Reflection;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Models;
using Audora.Domain.Entities;

namespace Audora.Application.Search;

public static class SearchFilterExtensions
{
    public static IQueryable<T> ApplySorting<T>(this IQueryable<T> podcasts, string? sortField,
        SortOrder? sortOrder)
    {
        if (string.IsNullOrWhiteSpace(sortField))
            return podcasts;

        var podcastType = typeof(T);

        var sortFieldProperty = podcastType.GetProperty(sortField,
            BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (sortFieldProperty is null)
            return podcasts;

        var parameter = Expression.Parameter(podcastType, "p");
        var property = Expression.Property(parameter, sortFieldProperty);
        var delegateType = typeof(Func<,>).MakeGenericType(podcastType, property.Type);
        var lambda = Expression.Lambda(delegateType, property, parameter);

        return sortOrder == SortOrder.Ascending
            ? Queryable.OrderBy(podcasts, (dynamic)lambda)
            : Queryable.OrderByDescending(podcasts, (dynamic)lambda);
    }

    public static IQueryable<TSource> FilterBy<TSource, TValue>(this IQueryable<TSource> source,
        Expression<Func<TSource, TValue>> propertySelector, TValue? value)
    {
        if (value is null || (value is string str && string.IsNullOrWhiteSpace(str)))
            return source;

        var parameter = propertySelector.Parameters[0];
        var member = propertySelector.Body;

        Expression predicate;

        if (typeof(TValue) == typeof(string))
        {
            var toLowerMethod = typeof(string).GetMethod(nameof(string.ToLower), Type.EmptyTypes);
            var containsMethod = typeof(string).GetMethod(nameof(string.Contains), [typeof(string)]);

            var propertyToLower = Expression.Call(member, toLowerMethod!);
            var valueToLower = Expression.Constant(((string)(object)value!).ToLower());

            predicate = Expression.Call(propertyToLower, containsMethod!, valueToLower);
        }
        else
            predicate = Expression.Equal(member, Expression.Constant(value, typeof(TValue)));

        var lambda = Expression.Lambda<Func<TSource, bool>>(predicate, parameter);
        return source.Where(lambda);
    }


    public static IQueryable<Podcast> FilterByTags(this IQueryable<Podcast> podcasts, string[]? tags)
    {
        if (tags is null || tags.Length == 0)
            return podcasts;

        return podcasts.Where(podcast => podcast.Tags.Any(tag => tags.Contains(tag.Name)));
    }

    public static IQueryable<Podcast> FilterByRating(this IQueryable<Podcast> podcasts, float? rating)
    {
        return rating is null ? podcasts : podcasts.Where(podcast => podcast.AverageRating >= rating);
    }

    public static IQueryable<Podcast> FilterByCreator(this IQueryable<Podcast> podcasts, IQueryable<User> users,
        string? creator)
    {
        if (string.IsNullOrWhiteSpace(creator))
            return podcasts;

        // TODO improve and optimize search mechanism.
        var creatorsIds = users.Where(c => c.Name.Contains(creator)).Select(c => c.Id);

        return podcasts.Where(podcast => creatorsIds.Contains(podcast.CreatorId));
    }
}