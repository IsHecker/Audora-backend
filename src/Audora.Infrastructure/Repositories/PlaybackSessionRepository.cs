using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PlaybackSessionRepository : Repository<PlaybackSession, IPlaybackSessionRepository>, IPlaybackSessionRepository
{
    public PlaybackSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IQueryable<PlaybackSession>> GetAllByListenerId(Guid listenerId)
    {
        return (await base.GetAllAsync()).Where(ps => ps.ListenerId == listenerId);
    }

    public async Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId)
    {
        return await Query.Where(ps => ps.EpisodeId == episodeId && ps.ListenerId == listenerId)
                        .OrderByDescending(p => p.LastPlayedAt)
                        .FirstOrDefaultAsync();
    }
}