using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IAudioFileRepository : IRepository<AudioFile, IAudioFileRepository>
{
    Task<Dictionary<Guid, AudioFile>> GetByIdsAsync(IEnumerable<Guid> audioFileIds);
}