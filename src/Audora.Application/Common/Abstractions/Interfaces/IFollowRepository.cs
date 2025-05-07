using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IFollowRepository
{
    Task<IQueryable<Follow>> GetAllByEntityIdAsync(Guid entityId);
    Task<IQueryable<Follow>> GetListenerFollows(Guid followerId);
    Task<IQueryable<Follow>> GetListenerFollowsByEntityIds(Guid followerId, IQueryable<Guid> entityIds);
    Task AddAsync(Follow follow);
    Task<bool> DeleteAsync(Follow follow);

    Task<bool> IsListenerFollowing(Guid followerId, Guid entityId);
}