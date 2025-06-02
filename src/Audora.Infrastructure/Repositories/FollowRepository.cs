using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class FollowRepository : Repository<Follow, IFollowRepository>, IFollowRepository
{
    public FollowRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Follow>> GetAllByEntityIdAsync(Guid entityId)
    {
        return Task.FromResult(Query.Where(x => x.EntityId == entityId));
    }

    public Task<IQueryable<Follow>> GetListenerFollows(Guid followerId, EntityType entityType)
    {
        return Task.FromResult(Query.Where(x => x.ListenerId == followerId && x.EntityType == entityType));
    }

    public async Task<IQueryable<Follow>> GetListenerFollowsByEntityIds(Guid followerId, EntityType entityType, IEnumerable<Guid> entityIds)
    {
        return (await GetListenerFollows(followerId, entityType)).Where(f => entityIds.Contains(f.EntityId));
    }

    public async Task<bool> IsListenerFollowingAsync(Guid followerId, Guid entityId)
    {
        return await Query.FirstOrDefaultAsync(f => f.ListenerId == followerId && f.EntityId == entityId) is not null;
    }
}