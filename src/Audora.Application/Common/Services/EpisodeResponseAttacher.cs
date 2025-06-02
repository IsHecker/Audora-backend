using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Mappings;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Services;

// TODO CRITICAL manage response in a flexible and attaching way.
public class EpisodeResponseAttacher : ResponseAttacher<EpisodeResponseAttacher, EpisodeResponse>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly EngagementStatsService _engagementStatsService;

    private HashSet<Guid>? _episodeIdsCache;

    public EpisodeResponseAttacher(
        IEpisodeStatRepository episodeStatRepository,
        IReactionRepository reactionRepository,
        EngagementStatsService engagementStatsService)
    {
        _episodeStatRepository = episodeStatRepository;
        _reactionRepository = reactionRepository;
        _engagementStatsService = engagementStatsService;
    }

    private HashSet<Guid> EpisodeIds =>
          _episodeIdsCache ??= ResponseCollection.Select(p => p.Id).ToHashSet();


    public EpisodeResponseAttacher AttachEpisodeStats() =>
        Attach(
            AttachEpisodeStatsForOneAsync,
            AttachEpisodeStatsForAllAsync
        );

    private async Task AttachEpisodeStatsForAllAsync()
    {
        var episodeStatDict = (await _episodeStatRepository.GetAllByEpisodeIdsAsync(EpisodeIds))
                    .ToDictionary(es => es.EpisodeId);

        var engagementStatResult = await _engagementStatsService.GetStatsAsync(EpisodeIds, EntityType.Episode);

        if (engagementStatResult.IsError)
            return;


        AddAttachment(response =>
        {
            response.EpisodeStat = episodeStatDict[response.Id]
            .ToResponse(engagementStatResult.Value[response.Id]);
        });
    }

    private async Task AttachEpisodeStatsForOneAsync()
    {
        var episodeStat = await _episodeStatRepository.GetByEpisodeIdAsync(SingleResponse.Id);

        var engagementStatResult = await _engagementStatsService.GetStatsAsync(SingleResponse.Id, EntityType.Episode);

        if (engagementStatResult.IsError)
            return;

        SingleResponse.EpisodeStat = episodeStat.ToResponse(engagementStatResult.Value);
    }


    public EpisodeResponseAttacher AttachListenerReactions(Guid listenerId) =>
        Attach(
            () => AttachReactionForOneAsync(listenerId),
            () => AttachReactionsForAllAsync(listenerId)
        );

    private async Task AttachReactionsForAllAsync(Guid listenerId)
    {
        var dict = (await _reactionRepository.GetAllByEntityIdsAsync(EpisodeIds))
            .Where(r => r.ListenerId == listenerId)
            .ToDictionary(es => es.EntityId);

        AddAttachment(response =>
        {
            response.ListenerReaction = dict.TryGetValue(response.Id, out var reaction)
            ? reaction.ToResponse()
            : null;
        });
    }

    private async Task AttachReactionForOneAsync(Guid listenerId)
    {
        var reaction = await _reactionRepository.GetAsync(listenerId, SingleResponse.Id);

        SingleResponse.ListenerReaction = reaction?.ToResponse();
    }
}