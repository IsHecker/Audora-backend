using Audora.Application.Common.Abstractions.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public abstract class BaseRepository<TDomain, TRepository>
    : IBaseRepository<TDomain, TRepository>
    where TDomain : class
    where TRepository : IBaseRepository<TDomain, TRepository>
{
    protected readonly ApplicationDbContext Context;
    protected IQueryable<TDomain> Query { get; set; }

    protected BaseRepository(ApplicationDbContext context)
    {
        Context = context;
        Query = Context.Set<TDomain>();
    }


    public virtual Task<IQueryable<TDomain>> GetAllAsync()
    {
        return Task.FromResult(Query);
    }

    public virtual async Task<TDomain> AddAsync(TDomain entity)
    {
        return (await Context.AddAsync(entity)).Entity;
    }

    public virtual Task UpdateAsync(TDomain entity)
    {
        Context.Update(entity);
        return Task.CompletedTask;
    }

    public TRepository AsTracking()
    {
        Query = Query.AsTracking();
        return (TRepository)(IBaseRepository<TDomain, TRepository>)this;
    }
}