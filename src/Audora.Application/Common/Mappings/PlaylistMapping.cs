using Audora.Contracts.Playlists.Requests;
using Audora.Contracts.Playlists.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PlaylistMapping
{
    public static Playlist ToDomain(this PlaylistRequest request, Guid listenerId)
    {
        return new Playlist
        (
            listenerId,
            request.Name,
            request.Description,
            request.IsPublic,
            request.CoverImageUrl
        );
    }

    public static PlaylistResponse ToResponse(this Playlist playlist)
    {
        return new PlaylistResponse
        {
            Id = playlist.Id,
            Name = playlist.Name,
            Description = playlist.Description,
            CoverImageUrl = playlist.CoverImageUrl,
        };
    }

    public static IEnumerable<PlaylistResponse> ToResponse(this IEnumerable<Playlist> playlists)
    {
        return playlists.Select(ToResponse);
    }
}