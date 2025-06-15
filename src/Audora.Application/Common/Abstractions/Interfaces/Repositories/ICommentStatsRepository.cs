using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories
{
    public interface ICommentStatRepository : IBaseRepository<CommentStat, ICommentStatRepository>
    {
        Task<CommentStat?> GetCommentStatAsync(Guid entityId, EntityType entityType);
        Task<Dictionary<Guid, CommentStat>> GetByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType);
        Task<Dictionary<Guid, int>> GetCommentCountByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType);

        Task<CommentStat> AddAsync(Guid entityId, EntityType entityType);

        Task<int> GetCommentCountAsync(Guid entityId, EntityType entityType);
        Task<bool> DeleteByEntityAsync(Guid entityId, EntityType entityType);
    }
}