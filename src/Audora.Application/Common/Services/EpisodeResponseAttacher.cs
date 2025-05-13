using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Mappings;
using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Services;

public class EpisodeResponseAttacher
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public EpisodeResponseAttacher(IEpisodeStatRepository episodeStatRepository,
        IReactionRepository reactionRepository, IEngagementStatRepository engagementStatRepository)
    {
        _episodeStatRepository = episodeStatRepository;
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
    }
    
    
    // public async Task<IEnumerable<EpisodeResponse>> AttachListenerMetadataAsync(List<Episode> episodes,
    //     Guid listenerId)
    // {
    //     return episodes.AsEnumerable().ToResponse(episodeStatsDict, engagementStatsDict, listenerReactionsDict);
    // }
    
    
    
    public async Task<EpisodeResponseAttacher> AttachEpisodeStat(List<EpisodeResponse> episodes)
    {
        var episodeIds = episodes.Select(p => p.Id).ToList();

        var episodeStatsDict = (await _episodeStatRepository.GetAllByEpisodeIdsAsync(episodeIds))
            .ToDictionary(es => es.EpisodeId);
        
        return this;
    }
    
    public async Task<EpisodeResponseAttacher> AttachListenerReactions(List<EpisodeResponse> episodes, Guid listenerId)
    {
        var episodeIds = episodes.Select(p => p.Id).ToList();
        
        var listenerReactionsDict = (await _reactionRepository.GetAllByEntityIdsAsync(episodeIds))
            .Where(r => r.ListenerId == listenerId)
            .ToDictionary(es => es.EntityId);
        
        return this;
    }
    
    public async Task<EpisodeResponseAttacher> AttachEngagementStat(List<EpisodeResponse> episodes, Guid listenerId)
    {
        var episodeIds = episodes.Select(p => p.Id).ToList();
        var engagementStatsDict = (await _engagementStatRepository.GetByEntityIdsAsync(episodeIds))
            .ToDictionary(es => es.EntityId);

        
        return this;
    }
    
    public async Task<EpisodeResponseAttacher> AttachEpisodeStat(EpisodeResponse episode)
    {
        var id = episode.Id;
        var episodeStats = await _episodeStatRepository.GetByEpisodeIdAsync(id);

        return this;
    }

    public async Task<EpisodeResponse> AttachListenerMetadataAsync(Episode episode, Guid listenerId)
    {
        var id = episode.Id;
        var episodeStats = await _episodeStatRepository.GetByEpisodeIdAsync(id);

        var listenerReactionsDict = await _reactionRepository.GetAsync(listenerId, id);

        var engagementStatsDict = await _engagementStatRepository.GetByEntityIdAsync(id);

        return episode.ToResponse(episodeStats, engagementStatsDict, listenerReactionsDict);
    }
}