using Audora.Contracts.Podcasts.Requests;
using Audora.Contracts.Podcasts.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class PodcastMapping
{
    public static Podcast ToDomain(this CreatePodcastRequest request)
    {
        return Podcast.Create(
            name: request.Name,
            description: request.Description,
            isPublished: request.IsPublished,
            category: request.Category,
            language: request.Language,
            tags: request.Tags,
            creatorId: request.CreatorId,
            coverImageUrl: request.CoverImageUrl);
    }

    public static Podcast ToDomain(this UpdatePodcastRequest request)
    {
        return Podcast.Create(
            name: request.Name,
            description: request.Description,
            isPublished: request.IsPublished,
            category: request.Category,
            language: request.Language,
            tags: request.Tags,
            creatorId: Guid.Empty,
            coverImageUrl: request.CoverImageUrl);
    }

    public static PodcastResponse ToResponse(this Podcast podcast)
    {
        return new PodcastResponse
        {
            Id = podcast.Id,
            CreatorId = podcast.CreatorId,
            Name = podcast.Name,
            Description = podcast.Description,
            CoverImageUrl = podcast.CoverImageUrl,
            Category = podcast.Category,
            Language = podcast.Language,
            TotalEpisodes = podcast.TotalEpisodes,
            AverageRating = podcast.AverageRating,
            TotalRatings = podcast.TotalRatings,
            Tags = podcast.Tags.Select(tag => tag.Name)
        };
    }

    public static IEnumerable<PodcastResponse> ToResponse(this IEnumerable<Podcast> podcasts)
    {
        return podcasts.Select(ToResponse);
    }
}