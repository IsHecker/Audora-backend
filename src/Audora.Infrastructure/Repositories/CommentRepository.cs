using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;

namespace Audora.Infrastructure.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Comment>> GetAllByEntityId(Guid episodeId)
    {
        return Task.FromResult(Query.Where(c => c.EntityId == episodeId));
    }
}