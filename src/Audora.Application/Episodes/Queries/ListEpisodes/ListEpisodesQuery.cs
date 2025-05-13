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

namespace Audora.Application.Episodes.Queries.ListEpisodes;

public record ListEpisodesQuery(
    Guid ParentId,
    string ParentType,
    Guid ListenerId,
    Pagination Pagination,
    bool Details = false)
    : IQuery<PagedResponse<EpisodeResponse>>;

public class ListEpisodesQueryHandler : IQueryHandler<ListEpisodesQuery, PagedResponse<EpisodeResponse>>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly EpisodeResponseAttacher _episodeResponseAttacher;

    public ListEpisodesQueryHandler(IEpisodeRepository episodeRepository,
        EpisodeResponseAttacher episodeResponseAttacher)
    {
        _episodeRepository = episodeRepository;
        _episodeResponseAttacher = episodeResponseAttacher;
    }

    public async Task<Result<PagedResponse<EpisodeResponse>>> Handle(ListEpisodesQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination;

        var episodes = (await GetEpisodes(request.ParentId, request.ParentType)).Paginate(pagination).ToList();

        // var response = request.Details
        //     ? await _episodeResponseAttacher.AttachListenerMetadataAsync(episodes, request.ListenerId)
        //     : episodes.Select(e => e.ToResponse());

        return episodes.Select(e => e.ToResponse())
            .ToPagedResponse(pagination.PageNumber, pagination.PageSize, episodes.Count);
    }

    private async Task<IQueryable<Episode>> GetEpisodes(Guid parentId, string parentType)
    {
        return parentType.ToLower() switch
        {
            "playlists" => await _episodeRepository.GetAllByPlaylistIdAsync(parentId),
            _ => await _episodeRepository.GetAllByPodcastIdAsync(parentId)
        };
    }
}