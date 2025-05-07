using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Mappings;
using Audora.Application.Common.Results;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Episodes.Queries.GetEpisode;

public record GetEpisodeQuery(Guid ListenerId, Guid EpisodeId) : IQuery<EpisodeResponse>;

public class GetEpisodeQueryHandler : IQueryHandler<GetEpisodeQuery, EpisodeResponse>
{
    private readonly IEpisodeRepository _episodeRepository;
    private readonly IEpisodeStatRepository _episodeStatRepository;
    private readonly IReactionRepository _reactionRepository;

    public GetEpisodeQueryHandler(IEpisodeRepository episodeRepository, IEpisodeStatRepository episodeStatRepository,
        IReactionRepository reactionRepository)
    {
        _episodeRepository = episodeRepository;
        _episodeStatRepository = episodeStatRepository;
        _reactionRepository = reactionRepository;
    }

    public async Task<Result<EpisodeResponse>> Handle(GetEpisodeQuery request, CancellationToken cancellationToken)
    {
        var episode = await _episodeRepository.GetByIdAsync(request.EpisodeId);
        if (episode is null)
        {
            return Error.NotFound(description: $"Episode with Id '{request.EpisodeId}' is not found.");
        }

        var episodeState = await _episodeStatRepository.GetByEpisodeIdAsync(request.EpisodeId);

        var listenerReaction = await _reactionRepository.GetAsync(request.ListenerId, request.EpisodeId);

        return episode.ToResponse(episodeState, listenerReaction);
    }
}