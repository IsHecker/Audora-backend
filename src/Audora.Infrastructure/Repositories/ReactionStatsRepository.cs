using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class ReactionStatsRepository : BaseRepository<ReactionStat, IReactionStatRepository>, IReactionStatRepository
{
    public ReactionStatsRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IEnumerable<ReactionStat>> GetByEntityAsync(Guid entityId, EntityType entityType)
    {
        return Task.FromResult<IEnumerable<ReactionStat>>(Query
            .Where(rs => rs.EntityId == entityId && rs.EntityType == entityType));
    }

    public async Task<ReactionStat?> GetReactionStatAsync(Guid entityId, EntityType entityType, ReactionType reactionType)
    {
        return await Query
            .FirstOrDefaultAsync(rs => rs.EntityId == entityId && rs.EntityType == entityType && rs.ReactionType == reactionType);
    }

    public Task<ReactionStat?> GetByReactionAsync(Reaction reaction)
    {
        return GetReactionStatAsync(reaction.EntityId, reaction.EntityType, reaction.ReactionType);
    }

    public async Task<bool> DeleteByEntityAsync(Guid entityId, EntityType entityType)
    {
        return await Query
            .Where(rs => rs.EntityId == entityId && rs.EntityType == entityType)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<Dictionary<Guid, List<ReactionStat>>> GetByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType)
    {
        return await Query
            .Where(rs => entityIds.Contains(rs.EntityId) && rs.EntityType == entityType)
            .GroupBy(rs => rs.EntityId)
            .ToDictionaryAsync(g => g.Key, g => g.ToList());
    }

    public async Task<ReactionStat> AddAsync(Guid entityId, EntityType entityType, ReactionType reactionType)
    {
        var reactionStat = new ReactionStat
        {
            EntityId = entityId,
            EntityType = entityType,
            ReactionType = reactionType
        };
        await Context.ReactionStats.AddAsync(reactionStat);
        return reactionStat;
    }

    public Task<ReactionStat> AddAsync(Reaction reaction)
    {
        return AddAsync(reaction.EntityId, reaction.EntityType, reaction.ReactionType);
    }

    public Task<bool> ExistsAsync(Reaction reaction)
    {
        return Query.AnyAsync(rs =>
            rs.EntityId == reaction.EntityId
            && rs.EntityType == reaction.EntityType
            && rs.ReactionType == reaction.ReactionType);
    }
}