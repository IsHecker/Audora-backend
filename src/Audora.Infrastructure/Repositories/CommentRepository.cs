using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment, ICommentRepository>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Comment>> GetByEntityAsync(Guid entityId, EntityType entityType)
    {
        return Task.FromResult(Query.Where(c => c.EntityId == entityId && c.EntityType == entityType));
    }

    public Task<IQueryable<Comment>> GetByParentCommentAsync(Guid parentId)
    {
        return Task.FromResult(Query.Where(c => c.ParentId == parentId));
    }
}