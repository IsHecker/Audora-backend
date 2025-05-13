using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Episode : Entity
{
    public string Name { get; init; } = null!;
    public Guid PodcastId { get; init; }
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public long Duration { get; init; }
    public Guid AudioFileId { get; init; }
    public string PodcastName { get; init; } = null!;
    public bool IsPublished { get; init; }
    public int EpisodeNumber { get; init; }
    public DateTime ReleaseDate { get; init; } // The date when it's published
    public string Slug => Name.ToLower().Replace(' ', '-');

    public ICollection<Playlist> Playlists { get; init; }

    public Episode(
        string name,
        Guid podcastId,
        string description,
        Guid audioFileId,
        string podcastName,
        bool isPublished,
        int episodeNumber,
        string? coverImageUrl)
    {
        Name = name;
        PodcastId = podcastId;
        Description = description;
        AudioFileId = audioFileId;
        PodcastName = podcastName;
        IsPublished = isPublished;
        EpisodeNumber = episodeNumber;
        CoverImageUrl = coverImageUrl;
    }

    private Episode(string? coverImageUrl)
    {
        CoverImageUrl = coverImageUrl;
    }
}