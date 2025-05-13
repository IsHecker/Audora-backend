using Audora.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public abstract class Repository<T> where T : Entity
{
    protected readonly ApplicationDbContext Context;

    protected IQueryable<T> Query { get; set; }

    protected Repository(ApplicationDbContext context)
    {
        Context = context;
        Query = Context.Set<T>();
    }

    public virtual Task<IQueryable<T>> GetAllAsync()
    {
        return Task.FromResult(Query);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await Query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual async Task AddAsync(T entity)
    {
        await Context.AddAsync(entity);
    }

    public virtual Task UpdateAsync(T entity)
    {
        Context.Update(entity);
        return Task.CompletedTask;
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        return await Query.Where(e => e.Id == id).ExecuteDeleteAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(T entity)
    {
        return await Query.Where(e => e.Id == entity.Id).ExecuteDeleteAsync() > 0;
    }
}