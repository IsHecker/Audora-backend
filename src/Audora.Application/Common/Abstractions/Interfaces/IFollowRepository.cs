using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IFollowRepository : IRepository<Follow>
{
    Task<IQueryable<Follow>> GetAllByEntityIdAsync(Guid entityId);
    Task<IQueryable<Follow>> GetListenerFollows(Guid followerId);
    Task<IQueryable<Follow>> GetListenerFollowsByEntityIds(Guid followerId, IEnumerable<Guid> entityIds);

    Task<bool> IsListenerFollowingAsync(Guid followerId, Guid entityId);
}