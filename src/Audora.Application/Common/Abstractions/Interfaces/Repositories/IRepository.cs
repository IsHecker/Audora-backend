namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IRepository<TEntity, TRepository>
    : IBaseRepository<TEntity, TRepository>
    where TRepository : IBaseRepository<TEntity, TRepository>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteAsync(IEnumerable<Guid> entityIds);
    Task<bool> DeleteAsync(IEnumerable<TEntity> entities);
    Task<bool> ExistsAsync(Guid id);
}