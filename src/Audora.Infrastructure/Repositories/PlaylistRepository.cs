using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;

namespace Audora.Infrastructure.Repositories;

public class PlaylistRepository : Repository<Playlist, IPlaylistRepository>, IPlaylistRepository
{
    public PlaylistRepository(ApplicationDbContext context) : base(context)
    {
    }
}