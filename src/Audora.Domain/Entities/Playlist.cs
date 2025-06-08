using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Playlist : Entity
{
    public Guid ListenerId { get; init; } // Listener who created the playlist.
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; } = null!;
    public bool IsPublic { get; init; }

    public ICollection<PlaylistEpisode> PlaylistEpisodes { get; init; } = [];

    public Playlist(
        Guid listenerId,
        string name,
        string description,
        bool isPublic,
        string? coverImageUrl = null)
    {
        ListenerId = listenerId;
        Name = name;
        Description = description;
        IsPublic = isPublic;
        CoverImageUrl = coverImageUrl;
    }

    private Playlist()
    {
    }

    public void AddEpisode(Guid episodeId, int order)
    {
        PlaylistEpisodes.Add(new PlaylistEpisode
        {
            PlaylistId = Id,
            EpisodeId = episodeId,
            Order = order,
            AddedAt = DateTime.UtcNow
        });
    }

    public void AddEpisodes(IEnumerable<Guid> episodeIds)
    {
        foreach (var episodeId in episodeIds)
        {
            PlaylistEpisodes.Add(new PlaylistEpisode { PlaylistId = Id, EpisodeId = episodeId });
        }
    }
}