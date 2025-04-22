using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Podcast : Entity
{
    private const int MaxTagsNumber = 10;

    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public bool IsPublished { get; private set; }
    public string Category { get; init; } = null!;
    public string Language { get; init; } = null!;
    public int TotalEpisodes { get; private set; }
    public string Slug { get; } = null!;     // TODO construct from podcast name.
    public string? RssFeedUrl { get; private set; }
    public Guid CreatorId { get; init; }

    public ICollection<Episode> Episodes { get; } = [];
    public ICollection<Tag> Tags { get; private set; } = [];

    public Podcast(
        string name,
        string description,
        bool isPublished,
        string category,
        string language,
        Guid creatorId,
        string? coverImageUrl = null)
    {
        Name = name;
        Description = description;
        CoverImageUrl = coverImageUrl;
        IsPublished = isPublished;
        Category = category;
        Language = language;
        CreatorId = creatorId;
    }

    private Podcast()
    {
    }

    public void Publish()
    {
        if (Episodes.Count < 1)
        {
            // TODO return error.
            return;
        }

        IsPublished = true;
    }

    public void Unpublish()
    {
        IsPublished = false;
    }

    public void AddTags(string[] tags)
    {
        // TODO maybe add tags repository to handle new tags.

        if (tags.Length > MaxTagsNumber)
        {
            // TODO return error.
            return;
        }

        Tags = tags.Select(tag => new Tag(tag)).ToList();
    }

    public void AddEpisode(Episode episode)
    {
        Episodes.Add(episode);
        TotalEpisodes++;
    }
}