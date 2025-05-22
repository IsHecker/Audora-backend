using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IReactionRepository : IRepository<Reaction>
{
    Task<IQueryable<Reaction>> GetAllByEntityIdsAsync(IEnumerable<Guid> entityIds);
    Task<Reaction?> GetAsync(Guid listenerId, Guid entityId);
}