using System.ComponentModel.DataAnnotations.Schema;
using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Podcast : Entity
{
    private const int MaxTagsNumber = 10;

    public Guid CreatorId { get; init; }
    public string Name { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public string? CoverImageUrl { get; private set; }
    public bool IsPublished { get; private set; }
    public string Category { get; private set; } = null!;
    public string Language { get; private set; } = null!;
    public int TotalEpisodes { get; private set; }
    public float AverageRating { get; private set; }
    public int TotalRatings { get; private set; }

    public string Slug => Name.ToLower().Replace(' ', '-');

    public ICollection<Episode>? Episodes { get; } = [];
    
    [NotMapped]
    public ICollection<Tag> Tags { get; private set; } = [];

    public static Podcast Create(
        string name,
        string description,
        bool isPublished,
        string category,
        string language,
        string[]? tags,
        Guid creatorId,
        string? coverImageUrl = null,
        ICollection<Episode>? episodes = null)
    {
        var podcast = new Podcast
        {
            Name = name,
            Description = description,
            CoverImageUrl = coverImageUrl,
            IsPublished = isPublished,
            Category = category,
            Language = language,
            CreatorId = creatorId
        };

        podcast.SetTags(tags ?? []); // TODO handle & return error.
        return podcast;
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

    public void SetTags(string[] tags)
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

    public void UpdateAverageRating(float newAverageRating) => AverageRating = newAverageRating;
    public void UpdateTotalRating(int newTotalRatings) => TotalRatings = newTotalRatings;

    public void Update(Podcast podcastToUpdate)
    {
        Name = podcastToUpdate.Name;
        Description = podcastToUpdate.Description;
        CoverImageUrl = podcastToUpdate.CoverImageUrl;
        IsPublished = podcastToUpdate.IsPublished;
        Category = podcastToUpdate.Category;
        Language = podcastToUpdate.Language;
        Tags = podcastToUpdate.Tags;
    }
}