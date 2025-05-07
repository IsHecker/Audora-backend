using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class ReactionRepository : Repository<Reaction>, IReactionRepository
{
    public ReactionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Reaction>> GetAllByEntityIdsAsync(IEnumerable<Guid> entityIds)
    {
        return Task.FromResult(Query.Where(r => entityIds.Contains(r.EntityId)));
    }

    public async Task<Reaction?> GetAsync(Guid listenerId, Guid entityId)
    {
        return await Query.FirstOrDefaultAsync(r => r.ListenerId == listenerId && r.EntityId == entityId);
    }
}