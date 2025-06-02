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
    private readonly IPodcastRepository _podcastRepository;
    private readonly IPlaylistRepository _playlistRepository;

    public ListEpisodesByParentIdQueryHandler(
        IEpisodeRepository episodeRepository,
        EpisodeResponseAttacher episodeResponseAttacher,
        IPodcastRepository podcastRepository,
        IPlaylistRepository playlistRepository)
    {
        _episodeRepository = episodeRepository;
        _episodeResponseAttacher = episodeResponseAttacher;
        _podcastRepository = podcastRepository;
        _playlistRepository = playlistRepository;
    }

    public async Task<Result<PagedResponse<EpisodeResponse>>> Handle(ListEpisodesByParentIdQuery request,
        CancellationToken cancellationToken)
    {
        var pagination = request.Pagination;

        var episodesResult = await GetParentEpisodes(request.ParentId, request.ParentType);

        if (episodesResult.IsError)
            return episodesResult.Errors;

        var episodes = episodesResult.Value;

        var response = episodes.Paginate(pagination).ToResponse().ToList();

        if (request.Details)
            response = _episodeResponseAttacher.AttachTo(response)
                .AttachEpisodeStats()
                .AttachListenerReactions(request.ListenerId)
                .GetResponseCollection();

        return response.ToPagedResponse(pagination, episodes.Count());
    }

    private async Task<Result<IQueryable<Episode>>> GetParentEpisodes(Guid parentId, string parentType)
    {
        switch (parentType.ToLower())
        {
            case "playlists":
                if (!await _playlistRepository.ExistsAsync(parentId))
                    return Error.NotFound(description: $"Playlist with Id '{parentId}' is not found.");

                return (await _episodeRepository.GetAllByPlaylistIdAsync(parentId)).ToResult();

            case "podcasts":
                if (!await _podcastRepository.ExistsAsync(parentId))
                    return Error.NotFound(description: $"Podcast with Id '{parentId}' is not found.");

                return (await _episodeRepository.GetAllByPodcastIdAsync(parentId)).ToResult();

            default:
                throw new NotImplementedException();
        }
    }
}