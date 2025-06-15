using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface ICommentRepository : IRepository<Comment, ICommentRepository>
{
    Task<IQueryable<Comment>> GetByEntityAsync(Guid entityId, EntityType entityType);
    Task<IQueryable<Comment>> GetByParentCommentAsync(Guid parentId);
}