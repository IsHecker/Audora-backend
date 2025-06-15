using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PlaylistEpisodeRepository : BaseRepository<PlaylistEpisode, IPlaylistEpisodeRepository>, IPlaylistEpisodeRepository
{
    public PlaylistEpisodeRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<bool> DeleteAsync(Guid playlistId, IEnumerable<Guid> episodeIds)
    {
        return await Query.Where(e => e.PlaylistId == playlistId && episodeIds.Contains(e.EpisodeId)).ExecuteDeleteAsync() > 0;
    }
}