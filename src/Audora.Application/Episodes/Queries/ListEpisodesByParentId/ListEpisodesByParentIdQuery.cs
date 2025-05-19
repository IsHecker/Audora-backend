using Audora.Application.Common;
using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Models;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Contracts.Common;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Episodes.Queries.ListEpisodesByParentId;

public record ListEpisodesByParentIdQuery(
    Guid ParentId,
    string ParentType,
    Guid ListenerId,
    Pagination Pagination,
    bool Details = false)
    : IQuery<PagedResponse<EpisodeResponse>>;

public class ListEpisodesByParentIdQueryHandler : IQueryHandler<ListEpisodesByParentIdQuery, PagedResponse<EpisodeResponse>>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly EpisodeResponseAttacher _episodeResponseAttacher;

    public ListEpisodesByParentIdQueryHandler(IEpisodeRepository episodeRepository,
        EpisodeResponseAttacher episodeResponseAttacher)
    {
        _episodeRepository = episodeRepository;
        _episodeResponseAttacher = episodeResponseAttacher;
    }

    public async Task<Result<PagedResponse<EpisodeResponse>>> Handle(ListEpisodesByParentIdQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination;

        var episodes = await GetParentEpisodes(request.ParentId, request.ParentType);

        var response = episodes.Paginate(pagination).ToResponse().ToList();

        if (request.Details)
            response = _episodeResponseAttacher.AttachTo(response)
                .AttachEpisodeStats()
                .AttachListenerReactions(request.ListenerId)
                .GetResponseCollection();

        return response.ToPagedResponse(pagination, episodes.Count());
    }

    private async Task<IQueryable<Episode>> GetParentEpisodes(Guid parentId, string parentType)
    {
        return parentType.ToLower() switch
        {
            "playlists" => await _episodeRepository.GetAllByPlaylistIdAsync(parentId),
            "podcasts" => await _episodeRepository.GetAllByPodcastIdAsync(parentId),
            _ => throw new NotImplementedException()
        };
    }
}