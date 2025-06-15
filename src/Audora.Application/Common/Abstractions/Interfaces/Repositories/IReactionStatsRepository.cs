using Audora.Domain.Common.Enums;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories
{
    public interface IReactionStatRepository : IBaseRepository<ReactionStat, IReactionStatRepository>
    {
        Task<ReactionStat?> GetReactionStatAsync(Guid entityId, EntityType entityType, ReactionType reactionType);
        Task<ReactionStat?> GetByReactionAsync(Reaction reaction);
        Task<IEnumerable<ReactionStat>> GetByEntityAsync(Guid entityId, EntityType entityType);
        Task<Dictionary<Guid, List<ReactionStat>>> GetByEntitiesAsync(IEnumerable<Guid> entityIds, EntityType entityType);

        Task<ReactionStat> AddAsync(Guid entityId, EntityType entityType, ReactionType reactionType);
        Task<ReactionStat> AddAsync(Reaction reaction);

        // Delete all reaction stats for a given entity (if needed)
        Task<bool> DeleteByEntityAsync(Guid entityId, EntityType entityType);


        Task<bool> ExistsAsync(Reaction reaction);
    }
}