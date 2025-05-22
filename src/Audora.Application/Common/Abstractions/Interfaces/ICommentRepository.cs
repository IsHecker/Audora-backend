using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface ICommentRepository : IRepository<Comment>
{
    Task<IQueryable<Comment>> GetAllByEntityId(Guid episodeId);
}