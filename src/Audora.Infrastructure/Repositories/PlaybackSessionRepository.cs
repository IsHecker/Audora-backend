using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class PlaybackSessionRepository : Repository<PlaybackSession>, IPlaybackSessionRepository
{
    public PlaybackSessionRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PlaybackSession?> GetAsync(Guid listenerId, Guid episodeId)
    {
        return await Query.FirstOrDefaultAsync(ps => ps.ListenerId == listenerId && ps.EpisodeId == episodeId);
    }
}