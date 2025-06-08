using Audora.Domain.Entities;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IPlaylistEpisodeRepository : IBaseRepository<PlaylistEpisode, IPlaylistEpisodeRepository>
{
    Task<bool> DeleteAsync(Guid playlistId, IEnumerable<Guid> episodeIds);
}