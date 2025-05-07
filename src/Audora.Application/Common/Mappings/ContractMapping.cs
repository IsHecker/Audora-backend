using Audora.Contracts.Analytics.Responses;
using Audora.Contracts.Comments.Responses;
using Audora.Contracts.Episodes.Requests;
using Audora.Contracts.Episodes.Responses;
using Audora.Contracts.PlaybackSessions.Responses;
using Audora.Contracts.Podcasts.Requests;
using Audora.Contracts.Podcasts.Responses;
using Audora.Contracts.Reactions.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class ContractMapping
{
    public static Podcast ToPodcast(this CreatePodcastRequest request)
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

    public static Podcast ToPodcast(this UpdatePodcastRequest request)
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

    public static Episode ToEpisode(this CreateEpisodeRequest request)
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

    public static PodcastResponse ToResponse(this Podcast podcast, bool isFollowed, byte? userRating)
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
            UserRating = userRating,
            IsFollowed = isFollowed
        };
    }

    public static PodcastsResponse ToResponse(
        this IEnumerable<Podcast> podcasts,
        HashSet<Guid> isFollowingSet,
        Dictionary<Guid, byte> userRatingDict)
    {
        return new PodcastsResponse
        {
            Podcasts = podcasts.Select(p =>
            {
                var isFollowing = isFollowingSet.Contains(p.Id);
                byte? userRating = userRatingDict.TryGetValue(p.Id, out var value) ? value : null;
                return ToResponse(p, isFollowing, userRating);
            })
        };
    }

    public static CommentResponse ToResponse(this Comment comment, EngagementStat engagementStat,
        Reaction listenerReaction)
    {
        return new CommentResponse
        {
            Id = comment.Id,
            Content = comment.Content,
            EditedAt = comment.UpdatedAt,
            ParentId = comment.ParentId,
            PostedAt = comment.CreatedAt,
            ListenerId = comment.ListenerId,
            Engagements = engagementStat.ToResponse(listenerReaction)
        };
    }

    public static CommentsResponse ToResponse(
        this IQueryable<Comment> comments,
        Dictionary<Guid, EngagementStat> engagementStatsDict,
        Dictionary<Guid, Reaction> listenerReactionsDict)
    {
        return new CommentsResponse
        {
            Comments = comments.Select(comment =>
                comment.ToResponse(engagementStatsDict[comment.Id], listenerReactionsDict[comment.Id]))
        };
    }

    public static CreateSessionResponse ToResponse(this PlaybackSession session)
    {
        return new CreateSessionResponse
        {
            PlaybackPosition = session.PlaybackPosition,
            FinishedAt = session.FinishedAt,
            StartedAt = session.StartedAt,
            IsCompleted = session.IsCompleted,
            LastPlayedAt = session.LastPlayedAt,
            ListenedDuration = session.ListenedDuration
        };
    }

    public static EngagementStatResponse ToResponse(this EngagementStat engagementStat, Reaction listenerReaction)
    {
        return new EngagementStatResponse
        {
            Likes = engagementStat.Likes,
            Dislikes = engagementStat.Dislikes,
            Comments = engagementStat.Comments,
            ListenerReaction = listenerReaction.ToResponse()
        };
    }

    public static EpisodeAnalyticsResponse ToResponse(this EpisodeStat episodeStat)
    {
        return new EpisodeAnalyticsResponse
        {
            Comments = episodeStat.Comments,
            Likes = episodeStat.Likes,
            Dislikes = episodeStat.Dislikes,
            PlayCount = episodeStat.PlayCount,
            Downloads = episodeStat.Downloads,
            EpisodeName = episodeStat.Episode.Name,
            ListeningTime = episodeStat.ListeningTime,
            Replays = episodeStat.Replays
        };
    }

    public static PodcastAnalyticsResponse ToResponse(this PodcastStat podcastStat,
        IQueryable<EpisodeStat> episodeStats)
    {
        return new PodcastAnalyticsResponse
        {
            PodcastName = podcastStat.Podcast.Name,
            AverageRating = podcastStat.AverageRating,
            RetentionRate = podcastStat.RetentionRate,
            TotalFollowers = podcastStat.TotalFollowers,
            TotalListeningTime = podcastStat.TotalListeningTime,
            TotalPlays = podcastStat.TotalPlays,
            TotalRatings = podcastStat.TotalRatings,
            TotalReturningListeners = podcastStat.TotalReturningListeners,
            EpisodesAnalytics = episodeStats.Select(ep => ep.ToResponse())
        };
    }

    public static ListenerReactionResponse ToResponse(this Reaction reaction)
    {
        return new ListenerReactionResponse
        {
            Reaction = reaction.ReactionType.ToString(), EntityType = reaction.EntityType
        };
    }

    public static EpisodeResponse ToResponse(this Episode episode, EpisodeStat episodeStat, Reaction? listenerReaction)
    {
        return new EpisodeResponse
        {
            Name = episode.Name,
            Description = episode.Description,
            CoverImageUrl = episode.CoverImageUrl,
            PodcastName = episode.PodcastName,
            Duration = episode.Duration,
            AudioFileId = episode.AudioFileId,
            EpisodeNumber = episode.EpisodeNumber,
            ReleaseDate = episode.ReleaseDate,
            EngagementStat = episodeStat.ToResponse(listenerReaction)
        };
    }

    public static EpisodeEngagementStatResponse ToResponse(this EpisodeStat episodeStat, Reaction? listenerReaction)
    {
        return new EpisodeEngagementStatResponse
        {
            Likes = episodeStat.Likes,
            Dislikes = episodeStat.Dislikes,
            Comments = episodeStat.Comments,
            Bookmarks = episodeStat.Bookmarks,
            PlayCount = episodeStat.PlayCount,
            Shares = episodeStat.Shares,
            ListenerReaction = listenerReaction?.ToResponse(),
            IsTrending = false
        };
    }

    public static EpisodesSummaryResponse ToResponse(
        this IQueryable<Episode> episodes,
        Dictionary<Guid, EpisodeStat> episodeStatsDict)
    {
        return new EpisodesSummaryResponse
        {
            Episodes = episodes.Select(ep => new EpisodeSummaryResponse
            {
                Name = ep.Name,
                CoverImageUrl = ep.CoverImageUrl,
                AudioFileId = ep.AudioFileId,
                EpisodeNumber = ep.EpisodeNumber,
                Likes = episodeStatsDict[ep.Id].Likes,
                Dislikes = episodeStatsDict[ep.Id].Dislikes,
                PlayCount = episodeStatsDict[ep.Id].PlayCount,
                PodcastName = ep.PodcastName,
                Duration = ep.Duration,
                ReleaseDate = ep.ReleaseDate
            })
        };
    }
}