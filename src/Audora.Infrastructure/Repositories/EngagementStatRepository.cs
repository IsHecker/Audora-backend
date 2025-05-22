using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class EngagementStatRepository : Repository<EngagementStat>, IEngagementStatRepository
{
    public EngagementStatRepository(ApplicationDbContext context) : base(context)
    {
    }
    public async Task<EngagementStat?> GetByEntityIdAsync(Guid entityId)
    {
        return await Query.FirstOrDefaultAsync(es => es.EntityId == entityId);
    }

    public Task<IQueryable<EngagementStat>> GetByEntityIdsAsync(IEnumerable<Guid> entityIds)
    {
        return Task.FromResult(Query.Where(es => entityIds.Contains(es.EntityId)));
    }

    public async Task<bool> DeleteByEntityIdAsync(Guid entityId)
    {
        return await Query.Where(e => e.EntityId == entityId).ExecuteDeleteAsync() > 0;
    }
}