namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IRepository<TEntity, TRepository>
    : IBaseRepository<TEntity, TRepository>
    where TRepository : IBaseRepository<TEntity, TRepository>
{
    Task<TEntity?> GetByIdAsync(Guid id);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> ExistsAsync(Guid id);
}