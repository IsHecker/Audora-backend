namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IRepository<T>
{
    Task<IQueryable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> DeleteAsync(T entity);

    IRepository<T> AsTracking();
}