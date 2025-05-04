using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IReactionRepository
{
    Task<IQueryable<Reaction>> GetReactionsByEntityIdsAsync(IEnumerable<Guid> entityIds);
    Task<Reaction?> GetByListenerIdAsync(Guid listenerId);
    Task AddAsync(Reaction reaction);
    Task DeleteAsync(Reaction reaction);
}