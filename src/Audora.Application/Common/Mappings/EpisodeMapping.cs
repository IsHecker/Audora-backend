using Audora.Contracts.Episodes.Requests;
using Audora.Contracts.Episodes.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class EpisodeMapping
{
    public static Episode ToDomain(this CreateEpisodeRequest request)
    {
        return new Episode(
            podcastId: request.PodcastId,
            audioFileId: request.AudioFileId,
            name: request.Name,
            description: request.Description,
            coverImageUrl: request.CoverImageUrl,
            isPublished: request.IsPublished,
            podcastName: request.PodcastName,
            episodeNumber: request.EpisodeNumber
        );
    }


    public static EpisodeResponse ToSimpleResponse(this Episode episode)
    {
        return new EpisodeResponse
        {
            Id = episode.Id,
            Name = episode.Name,
            Description = episode.Description,
            CoverImageUrl = episode.CoverImageUrl,
            PodcastName = episode.PodcastName,
            Duration = episode.Duration,
            EpisodeNumber = episode.EpisodeNumber,
            ReleaseDate = episode.ReleaseDate
        };
    }

    public static EpisodeResponse ToResponse(this Episode episode)
    {
        return new EpisodeResponse
        {
            Id = episode.Id,
            Name = episode.Name,
            Description = episode.Description,
            CoverImageUrl = episode.CoverImageUrl,
            PodcastName = episode.PodcastName,
            Duration = episode.Duration,
            AudioFileId = episode.AudioFileId,
            EpisodeNumber = episode.EpisodeNumber,
            ReleaseDate = episode.ReleaseDate
        };
    }

    public static IEnumerable<EpisodeResponse> ToResponse(this IEnumerable<Episode> episodes)
    {
        return episodes.Select(ep =>
        {
            return ep.ToResponse();
        });
    }


    public static EpisodeResponse WithStats(this EpisodeResponse response, EpisodeStat stat, EngagementStat engagement)
    {
        response.EpisodeStat = stat.ToResponse(engagement);
        return response;
    }

    public static EpisodeResponse WithListenerReaction(this EpisodeResponse response, Reaction listenerReaction)
    {
        response.ListenerReaction = listenerReaction.ToResponse();
        return response;
    }
}