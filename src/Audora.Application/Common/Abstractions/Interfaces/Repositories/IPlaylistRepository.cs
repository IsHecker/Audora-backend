using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces.Repositories;

public interface IPlaylistRepository : IRepository<Playlist, IPlaylistRepository>
{
    Task<IQueryable<Playlist>> GetAllByListenerIdAsync(Guid listenerId);
}