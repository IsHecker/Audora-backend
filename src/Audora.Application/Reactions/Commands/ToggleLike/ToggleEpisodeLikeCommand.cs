using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Domain.Entities;
using MediatR;

namespace Audora.Application.Reactions.Commands.ToggleLike;

public record ToggleEpisodeLikeCommand(Reaction Reaction) : ICommand;

public class ToggleEpisodeLikeCommandHandler : ICommandHandler<ToggleEpisodeLikeCommand>
{
    private readonly ReactionTogglerService _reactionTogglerService;

    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public ToggleEpisodeLikeCommandHandler(
        IReactionRepository reactionRepository,
        ReactionTogglerService reactionTogglerService,
        IEngagementStatRepository engagementStatRepository)
    {
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
        _reactionTogglerService = reactionTogglerService;
    }

    public async Task<Result> Handle(ToggleEpisodeLikeCommand request, CancellationToken cancellationToken)
    {
        var isReactionOn = await _reactionTogglerService.ToggleReactionAsync(request.Reaction);
        var engagementStat = await _engagementStatRepository.GetByEntityIdAsync(request.Reaction.EntityId);

        if (engagementStat is null)
        {
            return Error.NotFound(description: $"EngagementStat with EntityId '{request.Reaction.EntityId}' is not found.");
        }

        if (isReactionOn)
        {
            engagementStat.AddLike();
            return Result.Success;
        }

        engagementStat.RemoveLike();

        return Result.Success;
    }
}