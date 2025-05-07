using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IReactionRepository
{
    Task<IQueryable<Reaction>> GetAllByEntityIdsAsync(IEnumerable<Guid> entityIds);
    Task<Reaction?> GetAsync(Guid listenerId, Guid entityId);
    Task AddAsync(Reaction reaction);
    Task<bool> DeleteAsync(Reaction reaction);
}