using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IFollowRepository : IRepository<Follow, IFollowRepository>
{
    Task<IQueryable<Follow>> GetAllByEntityIdAsync(Guid entityId);
    Task<IQueryable<Follow>> GetListenerFollows(Guid followerId, EntityType entityType);
    Task<IQueryable<Follow>> GetListenerFollowsByEntityIds(Guid followerId, EntityType entityType, IEnumerable<Guid> entityIds);

    Task<bool> IsListenerFollowingAsync(Guid followerId, Guid entityId);
}