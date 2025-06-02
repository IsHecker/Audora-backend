namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IBaseRepository<TDomain, TRepository>
{
    Task<IQueryable<TDomain>> GetAllAsync();
    Task<TDomain> AddAsync(TDomain entity);
    Task UpdateAsync(TDomain entity);
    TRepository AsTracking();
}