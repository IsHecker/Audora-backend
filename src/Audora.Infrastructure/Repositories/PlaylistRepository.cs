using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Entities;

namespace Audora.Infrastructure.Repositories;

public class PlaylistRepository : Repository<Playlist, IPlaylistRepository>, IPlaylistRepository
{
    public PlaylistRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<IQueryable<Playlist>> GetAllByListenerIdAsync(Guid listenerId)
    {
        return Task.FromResult(Query.Where(pl => pl.ListenerId == listenerId));
    }
}