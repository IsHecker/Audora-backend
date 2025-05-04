using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Application.Common.Services;
using Audora.Domain.Entities;

namespace Audora.Application.Reactions.Commands.ToggleDislike;

public record ToggleEpisodeDislikeCommand(Reaction Reaction) : ICommand;

public class ToggleEpisodeDislikeCommandHandler : ICommandHandler<ToggleEpisodeDislikeCommand>
{
    private readonly ReactionTogglerService _reactionTogglerService;

    private readonly IReactionRepository _reactionRepository;
    private readonly IEngagementStatRepository _engagementStatRepository;

    public ToggleEpisodeDislikeCommandHandler(
        IReactionRepository reactionRepository,
        ReactionTogglerService reactionTogglerService,
        IEngagementStatRepository engagementStatRepository)
    {
        _reactionRepository = reactionRepository;
        _engagementStatRepository = engagementStatRepository;
        _reactionTogglerService = reactionTogglerService;
    }

    public async Task<Result> Handle(ToggleEpisodeDislikeCommand request, CancellationToken cancellationToken)
    {
        var isReactionSet = await _reactionTogglerService.ToggleReactionAsync(request.Reaction);
        var engagementStat = await _engagementStatRepository.GetByEntityIdAsync(request.Reaction.EntityId);

        if (engagementStat is null)
        {
            return Error.NotFound(description: $"EngagementStat with EntityId '{request.Reaction.EntityId}' is not found.");
        }

        if (isReactionSet)
        {
            engagementStat.AddDislike();
            return Result.Success;
        }

        engagementStat.RemoveDislike();

        return Result.Success;
    }
}