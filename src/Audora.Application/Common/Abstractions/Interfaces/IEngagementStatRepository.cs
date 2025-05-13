using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IEngagementStatRepository
{
    Task<IQueryable<EngagementStat>> GetAllAsync();
    Task<EngagementStat?> GetByEntityIdAsync(Guid entityId);
    Task<IQueryable<EngagementStat>> GetByEntityIdsAsync(IEnumerable<Guid> entityIds);
}