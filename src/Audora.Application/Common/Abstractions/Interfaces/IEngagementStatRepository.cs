using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEngagementStatRepository : IRepository<EngagementStat>
{
    Task<EngagementStat?> GetByEntityIdAsync(Guid entityId);
    Task<IQueryable<EngagementStat>> GetByEntityIdsAsync(IEnumerable<Guid> entityIds);
    Task<bool> DeleteByEntityIdAsync(Guid entityId);
}