using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class CommentStatRepository : BaseRepository<CommentStat, ICommentStatRepository>, ICommentStatRepository
{
    public CommentStatRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<int> GetCommentCountAsync(Guid entityId, EntityType entityType)
    {
        var stat = await Query
            .FirstOrDefaultAsync(cs => cs.EntityId == entityId && cs.EntityType == entityType);

        return stat?.CommentCount ?? 0;
    }

    public async Task<CommentStat?> GetCommentStatAsync(Guid entityId, EntityType entityType)
    {
        return await Query
            .FirstOrDefaultAsync(cs => cs.EntityId == entityId && cs.EntityType == entityType);
    }

    public async Task<bool> DeleteByEntityAsync(Guid entityId, EntityType entityType)
    {
        return await Query
            .Where(cs => cs.EntityId == entityId && cs.EntityType == entityType)
            .ExecuteDeleteAsync() > 0;
    }

    public async Task<Dictionary<Guid, CommentStat>> GetByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType)
    {
        return await Query
            .Where(cs => entityIds.Contains(cs.EntityId) && cs.EntityType == entityType)
            .ToDictionaryAsync(cs => cs.EntityId);
    }

    public async Task<Dictionary<Guid, int>> GetCommentCountByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType)
    {
        return await Query
            .Where(cs => entityIds.Contains(cs.EntityId) && cs.EntityType == entityType)
            .ToDictionaryAsync(cs => cs.EntityId, cs => cs.CommentCount);
    }

    public async Task<CommentStat> AddAsync(Guid entityId, EntityType entityType)
    {
        var commentStat = new CommentStat
        {
            EntityId = entityId,
            EntityType = entityType
        };
        await Context.CommentStats.AddAsync(commentStat);
        return commentStat;
    }
}