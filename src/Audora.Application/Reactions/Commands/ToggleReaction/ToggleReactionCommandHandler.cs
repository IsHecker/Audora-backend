using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Abstractions.Messaging;
using Audora.Application.Common.Results;
using Audora.Domain.Entities;

namespace Audora.Application.Reactions.Commands.ToggleReaction;

public record ToggleReactionCommand(Reaction Reaction) : ICommand;

public class ToggleReactionCommandHandler : ICommandHandler<ToggleReactionCommand>
{
  private readonly IReactionRepository _reactionRepository;
  private readonly IReactionStatRepository _reactionStatRepository;

  public ToggleReactionCommandHandler(
      IReactionRepository reactionRepository,
      IReactionStatRepository reactionStatRepository)
  {
    _reactionRepository = reactionRepository;
    _reactionStatRepository = reactionStatRepository;
  }

  public async Task<Result> Handle(ToggleReactionCommand request, CancellationToken cancellationToken)
  {
    var newReaction = request.Reaction;

    var listenerReaction = await _reactionRepository.AsTracking()
        .GetAsync(newReaction.ListenerId, newReaction.EntityId);

    var reactionStat = await _reactionStatRepository.AsTracking()
        .GetByReactionAsync(newReaction);

    if (reactionStat is null)
    {
      reactionStat = await _reactionStatRepository.AddAsync(newReaction);
    }

    if (listenerReaction is null)
    {
      await _reactionRepository.AddAsync(newReaction);
      reactionStat.IncreaseCount();
      return Result.Success;
    }

    if (newReaction.ReactionType != listenerReaction.ReactionType)
    {
      await SwapReaction(listenerReaction, reactionStat);
      return Result.Success;
    }

    if (reactionStat is null || reactionStat.Count < 1)
      return Error.NotFound(description: "No more Reactions to remove.");

    await _reactionRepository.DeleteAsync(listenerReaction);
    reactionStat.DecreaseCount();

    return Result.Success;
  }

  private async Task SwapReaction(Reaction listenerReaction, ReactionStat targetReactionStat)
  {
    var oldReactionStat = await _reactionStatRepository.GetByReactionAsync(listenerReaction)
      ?? throw new InvalidOperationException("Old reaction stat not found for the listener's current reaction.");

    targetReactionStat.IncreaseCount();
    oldReactionStat.DecreaseCount();

    listenerReaction.UpdateReactionType(targetReactionStat.ReactionType);
  }
}