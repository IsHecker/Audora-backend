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

    public static EpisodeResponse ToResponse(this Episode episode,
        EpisodeStat? episodeStat = null,
        EngagementStat? engagementStat = null,
        Reaction? listenerReaction = null)
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
            ReleaseDate = episode.ReleaseDate,
            ListenerReaction = listenerReaction?.ToResponse(),
            EpisodeStat = episodeStat?.ToResponse(engagementStat!),
        };
    }

    public static IEnumerable<EpisodeResponse> ToResponse(this IEnumerable<Episode> episodes,
        Dictionary<Guid, EpisodeStat>? episodeStats = null,
        Dictionary<Guid, EngagementStat>? engagementStats = null,
        Dictionary<Guid, Reaction>? listenerReactions = null)
    {
        return episodes.Select(ep =>
        {
            EpisodeStat? stat = null;
            EngagementStat? engagement = null;
            Reaction? reaction = null;

            episodeStats?.TryGetValue(ep.Id, out stat);
            engagementStats?.TryGetValue(ep.Id, out engagement);
            listenerReactions?.TryGetValue(ep.Id, out reaction);

            return ep.ToResponse(stat, engagement, reaction);
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