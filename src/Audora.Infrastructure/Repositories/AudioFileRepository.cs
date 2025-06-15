using Audora.Application.Common.Abstractions.Interfaces.Repositories;
using Audora.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Repositories;

public class AudioFileRepository : Repository<AudioFile, IAudioFileRepository>, IAudioFileRepository
{
    public AudioFileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public Task<Dictionary<Guid, AudioFile>> GetByIdsAsync(IEnumerable<Guid> audioFileIds)
    {
        return Query.Where(af => audioFileIds.Contains(af.Id)).ToDictionaryAsync(af => af.Id);
    }
}