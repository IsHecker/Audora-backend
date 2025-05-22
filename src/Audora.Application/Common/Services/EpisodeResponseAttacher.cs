using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Mappings;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Services;

// TODO CRITICAL manage response in a flexible and attaching way.
public class EpisodeResponseAttacher : ResponseAttacher<EpisodeResponseAttacher, EpisodeResponse>
{
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    private HashSet<Guid>? _episodeIdsCache;
    private HashSet<Guid> EpisodeIds =>
        _episodeIdsCache ??= ResponseCollection.Select(p => p.Id).ToHashSet();

    public EpisodeResponseAttacher(IEpisodeStatRepository episodeStatRepository,
        IReactionRepository reactionRepository, IEngagementStatRepository engagementStatRepository)
    {
        _episodeStatRepository = episodeStatRepository;
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
    }


    public EpisodeResponseAttacher AttachEpisodeStats() =>
        Attach(
            AttachEpisodeStatsForOneAsync,
            AttachEpisodeStatsForAllAsync
        );

    private async Task AttachEpisodeStatsForAllAsync()
    {
        var episodeStatDict = (await _episodeStatRepository.GetAllByEpisodeIdsAsync(EpisodeIds))
                    .ToDictionary(es => es.EpisodeId);

        var engagementStatDict = (await _engagementStatRepository.GetByEntityIdsAsync(EpisodeIds))
            .ToDictionary(es => es.EntityId);


        AddAttachment(response =>
        {
            response.EpisodeStat = episodeStatDict[response.Id]
            .ToResponse(engagementStatDict[response.Id]);
        });
    }

    private async Task AttachEpisodeStatsForOneAsync()
    {
        var episodeStat = await _episodeStatRepository.GetByEpisodeIdAsync(SingleResponse.Id);

        var engagementStat = await _engagementStatRepository.GetByEntityIdAsync(SingleResponse.Id);

        SingleResponse.EpisodeStat = episodeStat.ToResponse(engagementStat!);
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