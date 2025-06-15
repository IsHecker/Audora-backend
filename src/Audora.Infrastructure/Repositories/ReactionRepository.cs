using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class ReactionRepository : Repository<Reaction, IReactionRepository>, IReactionRepository
{
    public ReactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Reaction>> GetAllByEntityAsync(Guid entityId, EntityType entityType)
    {
        return Task.FromResult(Query.Where(r => r.EntityId == entityId && r.EntityType == entityType));
    }

    public Task<IQueryable<Reaction>> GetAllByEntityIdsAsync(IEnumerable<Guid> entityIds)
    {
        return Task.FromResult(Query.Where(r => entityIds.Contains(r.EntityId)));
    }

    public async Task<Reaction?> GetAsync(Guid listenerId, Guid entityId, EntityType entityType)
    {
        return await Query.FirstOrDefaultAsync(r =>
            r.ListenerId == listenerId
            && r.EntityId == entityId
            && r.EntityType == entityType);
    }
}