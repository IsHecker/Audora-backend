using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist, IPlaylistRepository>
{
    Task<IQueryable<Playlist>> GetAllByListenerIdAsync(Guid listenerId);
}