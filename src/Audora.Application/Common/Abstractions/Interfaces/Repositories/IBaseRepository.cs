namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IBaseRepository<TDomain, TRepository>
{
    Task<IQueryable<TDomain>> GetAllAsync();
    Task<TDomain> AddAsync(TDomain entity);
    Task<IEnumerable<TDomain>> AddAsync(IEnumerable<TDomain> entities);
    Task UpdateAsync(TDomain entity);
    TRepository AsTracking();
}