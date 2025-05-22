using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Episode : Entity
{
    public Guid PodcastId { get; init; }
    public string PodcastName { get; init; } = null!;
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? CoverImageUrl { get; private set; }
    public long Duration { get; private set; }
    public Guid AudioFileId { get; private set; }
    public bool IsPublished { get; private set; }
    public int EpisodeNumber { get; init; }
    public DateTime ReleaseDate { get; init; } // The date when it's published
    public string Slug => Name.ToLower().Replace(' ', '-');

    public ICollection<Playlist> Playlists { get; init; } = null!;

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

    private Episode()
    {
    }

    public void Update(Episode updatedEpisode)
    {
        Name = updatedEpisode.Name;
        Description = updatedEpisode.Description;
        CoverImageUrl = updatedEpisode.CoverImageUrl;
        AudioFileId = updatedEpisode.AudioFileId;
        Duration = updatedEpisode.Duration;
        IsPublished = updatedEpisode.IsPublished;
    }
}