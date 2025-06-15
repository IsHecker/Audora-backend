using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.Podcasts.Responses;
using Audora.Contracts.Search.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Search;

public record SearchQuery(SearchFilter Filter, Pagination Pagination, bool IsMixed = true) : IQuery<SearchResponse>;

public class SearchQueryHandler : IQueryHandler<SearchQuery, SearchResponse>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IUserService _userService;

    private readonly static int[] SharedPageSize = new int[2];

    public SearchQueryHandler(IPodcastRepository podcastRepository, IEpisodeRepository episodeRepository,
        IUserService userService)
    {
        _podcastRepository = podcastRepository;
        _episodeRepository = episodeRepository;
        _userService = userService;
    }

    public async Task<Result<SearchResponse>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        var pagination = request.Pagination;

        SharePageSizeRandomly(request.Pagination.PageSize);

        var (podcasts, podcastsCount) = await SearchPodcasts(filter, pagination);
        var (episodes, episodesCount) = await SearchEpisodes(filter, pagination);

        if (!request.IsMixed)
            return new SearchResponse { Podcasts = podcasts.ToResponse(), Episodes = episodes.ToResponse() };


        var mixedResults = new List<SearchResultItem>();

        mixedResults.AddRange(podcasts.Select(p => new SearchResultItem
        {
            Type = "podcast",
            Data = p.ToResponse()
        }));

        mixedResults.AddRange(episodes.Select(ep => new SearchResultItem
        {
            Type = "episode",
            Data = ep.ToResponse()
        }));

        // Shuffle the results
        var random = new Random();
        mixedResults = mixedResults.OrderBy(_ => random.Next()).ToList();

        return new SearchResponse
        {
            MixedResults = mixedResults.ToPagedResponse(request.Pagination, podcastsCount + episodesCount)
        };
    }

    private static void SharePageSizeRandomly(int pageSize)
    {
        var length = SharedPageSize.Length;
        var random = new Random();
        for (int i = 0; i < length - 1; i++)
        {
            var pg = random.Next(1, pageSize);
            pageSize -= pg;
            SharedPageSize[i] = pg;
        }
        SharedPageSize[^1] = pageSize;
    }

    private static (IQueryable<T> Items, int TotalCount) ApplySortingAndPaging<T>(
        IQueryable<T> results,
        SearchFilter filter,
        int pageNumber,
        int pageSize)
    {
        var totalCount = results.Count();

        var sorted = results.ApplySorting(filter.SortField, filter.SortOrder);
        var pagedResults = sorted.Paginate(pageNumber, pageSize);

        return (pagedResults, totalCount);
    }

    private async Task<(IQueryable<Podcast> Items, int TotalCount)> SearchPodcasts(SearchFilter filter, Pagination pagination)
    {
        var podcasts = (await _podcastRepository.GetAllAsync())
            .FilterBy(p => p.Name, filter.Name)
            .FilterBy(p => p.Category, filter.Category)
            .FilterBy(p => p.Language, filter.Language)
            .FilterByRating(filter.Rating)
            .FilterByTags(filter.Tags)
            .FilterByCreator(await _userService.GetUsersAsync(), filter.Creator);

        return ApplySortingAndPaging(podcasts, filter, pagination.PageNumber, SharedPageSize[0]);
    }

    private async Task<(IQueryable<Episode> Items, int TotalCount)> SearchEpisodes(SearchFilter filter, Pagination pagination)
    {
        var episodes = (await _episodeRepository.GetAllAsync())
            .FilterBy(e => e.Name, filter.Name);

        return ApplySortingAndPaging(episodes, filter, pagination.PageNumber, SharedPageSize[1]);
    }
}