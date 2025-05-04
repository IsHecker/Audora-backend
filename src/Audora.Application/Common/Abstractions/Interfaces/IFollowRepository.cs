using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IFollowRepository
{
    Task<IQueryable<Follow>> GetFollowsByEntityIdAsync(Guid entityId);
    Task<IQueryable<Follow>> GetUserFollows(Guid followerId);
    Task<bool> AddAsync(Guid followerId, Guid entityId);
    Task<bool> DeleteFollowAsync(Guid followerId, Guid entityId);
}