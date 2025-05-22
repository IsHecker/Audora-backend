using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Search;

public record SearchQuery(SearchFilter Filter, Pagination Pagination, bool IsMixed = false) : IQuery<SearchResults>;

public class SearchQueryHandler : IQueryHandler<SearchQuery, SearchResults>
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

    public async Task<Result<SearchResults>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var filter = request.Filter;
        filter.Pagination = request.Pagination;

        SharePageSizeRandomly(request.Pagination.PageSize);

        var (podcasts, podcastsCount) = await SearchPodcasts(filter);
        var (episodes, episodesCount) = await SearchEpisodes(filter);

        if (!request.IsMixed)
            return new SearchResults { Podcasts = podcasts, Episodes = episodes };


        var mixedResults = new List<SearchResultItem>();

        mixedResults.AddRange(podcasts.Select(p => new SearchResultItem
        {
            Type = "podcast",
            Data = p
        }));

        mixedResults.AddRange(episodes.Select(ep => new SearchResultItem
        {
            Type = "episode",
            Data = ep
        }));

        // Shuffle the results
        var random = new Random();
        mixedResults = mixedResults.OrderBy(_ => random.Next()).ToList();

        return new SearchResults
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


    private async Task<(IQueryable<Podcast> Items, int TotalCount)> SearchPodcasts(SearchFilter filter)
    {
        var podcasts = (await _podcastRepository.GetAllAsync())
            .FilterBy(p => p.Name, filter.Name)
            .FilterBy(p => p.Category, filter.Category)
            .FilterBy(p => p.Language, filter.Language)
            .FilterByRating(filter.Rating)
            .FilterByTags(filter.Tags)
            .FilterByCreator(await _userService.GetUsersAsync(), filter.Creator);

        var totalCount = podcasts.Count();

        var sorted = podcasts.ApplySorting(filter.SortField, filter.SortOrder);
        var paged = sorted.Paginate(filter.Pagination.PageNumber, SharedPageSize[0]);

        return (paged, totalCount);
    }

    private async Task<(IQueryable<Episode> Items, int TotalCount)> SearchEpisodes(SearchFilter filter)
    {
        var episodes = (await _episodeRepository.GetAllAsync())
            .FilterBy(e => e.Name, filter.Name);

        var totalCount = episodes.Count();

        var sorted = episodes.ApplySorting(filter.SortField, filter.SortOrder);
        var paged = sorted.Paginate(filter.Pagination.PageNumber, SharedPageSize[1]);

        return (paged, totalCount);
    }

}