using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface ICommentRepository
{
    Task<IQueryable<Comment>> GetAllByEntityId(Guid episodeId);
    Task<Comment?> GetByIdAsync(Guid commentId);
    Task AddAsync(Comment comment);
    Task<bool> DeleteAsync(Guid commentId);
}