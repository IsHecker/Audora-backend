using Audora.Contracts.Playlists.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PlaylistMapping
{
    public static PlaylistResponse ToResponse(this Playlist playlist)
    {
        return new PlaylistResponse
        {
            ListenerId = playlist.ListenerId,
            Name = playlist.Name,
            Description = playlist.Description,
            CoverImageUrl = playlist.CoverImageUrl,
        };
    }
}