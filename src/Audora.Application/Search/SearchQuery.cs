using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using MediatR;

namespace Audora.Application.Search;

public record SearchQuery(SearchFilter Filter) : IQuery<SearchResults>;

public class SearchQueryHandler : IQueryHandler<SearchQuery, SearchResults>
{
    private readonly IPodcastRepository _podcastRepository;
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IUserService _userService;

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

        var podcasts = (await _podcastRepository.GetPodcastsAsync())
            .FilterBy(p => p.Name, filter.Name)
            .FilterBy(p => p.Category, filter.Category)
            .FilterBy(p => p.Language, filter.Language)
            .FilterByRating(filter.Rating)
            .FilterByTags(filter.Tags)
            .FilterByCreator(_userService, filter.Creator)
            .Paginate(filter.Pagination)
            .ApplySorting(filter.SortField, filter.SortOrder);

        var episodes = (await _episodeRepository.GetEpisodesAsync())
            .FilterBy(e => e.Name, filter.Name)
            .Paginate(filter.Pagination)
            .ApplySorting(filter.SortField, filter.SortOrder);

        return new SearchResults() { Podcasts = podcasts, Episodes = episodes };
    }
}