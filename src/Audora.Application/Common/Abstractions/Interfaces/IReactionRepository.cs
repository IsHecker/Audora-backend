using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IReactionRepository : IRepository<Reaction, IReactionRepository>
{
    Task<IQueryable<Reaction>> GetAllByEntityIdsAsync(IEnumerable<Guid> entityIds);
    Task<IQueryable<Reaction>> GetAllByEntityAsync(Guid entityId, EntityType entityType);
    Task<Reaction?> GetAsync(Guid listenerId, Guid entityId, EntityType entityType);
}