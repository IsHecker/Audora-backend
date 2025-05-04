using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface ICommentRepository
{
    Task<IQueryable<Comment>> GetCommentsByEntityId(Guid episodeId);
    Task<Comment?> GetByIdAsync(Guid commentId);
    Task AddAsync(Comment comment);
    Task DeleteAsync(Guid commentId);
}