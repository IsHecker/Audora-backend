using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public abstract class Repository<TEntity, TRepository>
    : BaseRepository<TEntity, TRepository>,
    IRepository<TEntity, TRepository>
    where TEntity : Entity
    where TRepository : IRepository<TEntity, TRepository>
{
    protected Repository(ApplicationDbContext context) : base(context)
    {
    }

    public virtual async Task<TEntity?> GetByIdAsync(Guid id)
    {
        return await Query.FirstOrDefaultAsync(entity => entity.Id == id);
    }

    public virtual async Task<bool> DeleteAsync(Guid id)
    {
        return await Query.Where(e => e.Id == id).ExecuteDeleteAsync() > 0;
    }

    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        return await DeleteAsync(entity.Id);
    }

    public async Task<bool> ExistsAsync(Guid id)
    {
        return await Query.AnyAsync(entity => entity.Id == id);
    }
}