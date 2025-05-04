using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Services;

public class ReactionTogglerService
{
    private readonly IReactionRepository _reactionRepository;

    public ReactionTogglerService(IReactionRepository reactionRepository)
    {
        _reactionRepository = reactionRepository;
    }

    public async Task<bool> ToggleReactionAsync(Reaction reaction)
    {
        var listenerReaction = await _reactionRepository.GetByListenerIdAsync(reaction.ListenerId);
        if (listenerReaction is null)
        {
            await _reactionRepository.AddAsync(reaction);
            return true;
        }

        await _reactionRepository.DeleteAsync(reaction);
        return false;
    }
}