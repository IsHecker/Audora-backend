namespace Audora.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    private const string PodcastsBase = $"{ApiBase}/podcasts";
    private const string EpisodesBase = $"{ApiBase}/episodes";
    private const string CreatorsBase = $"{ApiBase}/creators";
    private const string ListenersBase = $"{ApiBase}/listeners";
    private const string PlaylistsBase = $"{ApiBase}/playlists";
    private const string SearchBase = $"{ApiBase}/search";
    private const string RatingsBase = $"{ApiBase}/ratings";
    private const string CommentsBase = $"{ApiBase}/comments";
    private const string FollowsBase = $"{ApiBase}/follows";
    private const string PlaybackSessionsBase = $"{ApiBase}/playback-sessions";

    public static class Podcasts
    {
        public const string List = PodcastsBase; // Get a paginated list of podcasts (with filters like category, tags, creator, etc).
        public const string GetById = $"{PodcastsBase}/{{podcastId:guid}}";
        public const string GetBySlug = $"{PodcastsBase}/slug/{{slug}}";
        public const string ListPodcastEpisodes = $"{GetById}/episodes"; // List episodes in a specific podcast.
        public const string ListFollowers = $"{GetById}/followers";
        public const string GetStats = $"{GetById}/stats";

        public const string CreateEpisode = $"{GetById}/episodes";

        public const string Create = PodcastsBase;
        public const string Update = $"{GetById}";
        public const string Delete = $"{GetById}";
    }

    public static class Episodes
    {
        public const string List = EpisodesBase;
        public const string GetById = $"{EpisodesBase}/{{episodeId:guid}}";
        public const string GetBySlug = $"{EpisodesBase}/slug/{{slug:guid}}";
        public const string GetStats = $"{GetById}/stats";
        public const string ListComments = $"{GetById}/comments";
        public const string ReactOnEpisode = $"{GetById}/react";
        public const string GetReactions = $"{GetById}/reactions";

        public const string Create = EpisodesBase;
        public const string Update = $"{GetById}";
        public const string Delete = $"{GetById}";
    }

    public static class Playlists
    {
        public const string GetById = $"{PlaylistsBase}/{{playlistId:guid}}";
        public const string ListPlaylistEpisodes = $"{GetById}/episodes";
    }

    public static class Creators
    {
        public const string GetById = $"{CreatorsBase}/{{creatorId:guid}}";
        public const string ListCreatorPodcasts = $"{GetById}/podcasts";
    }

    public static class Listeners
    {
        public const string GetById = $"{ListenersBase}/{{listenerId:guid}}";
        public const string ListFollowedPodcasts = $"{GetById}/followed-podcasts";
        public const string ListListenerReactions = $"{GetById}/reactions";
        public const string GetListenerReactionForEntity = $"{GetById}/reactions/{{entityId:guid}}";
    }

    public static class Search
    {
        public const string GlobalSearch = SearchBase;
        public const string MixedSearch = $"{SearchBase}/mixed";
    }

    public static class EngagementStat
    {
        public const string GetEngagementStats = $"{ApiBase}/{{resourceType}}/{{entityId:guid}}/engagement";
    }

    public static class Ratings
    {
        public const string RatePodcast = $"{RatingsBase}/podcast/{{podcastId:guid}}";
    }

    public static class Comments
    {
        public const string GetById = $"{CommentsBase}/{{commentId:guid}}";
        public const string List = CommentsBase;
        public const string CommentOnEntity = $"{ApiBase}/{{resourceType}}/{{entityId:guid}}/comments";
        public const string ListResourceComments = $"{ApiBase}/{{resourceType}}/{{entityId:guid}}/comments";
        public const string ListCommentReplies = $"{CommentsBase}/{{parentId:guid}}/replies";
        public const string ReplyToComment = $"{ApiBase}/{{resourceType}}/{{entityId:guid}}/comments/{{parentId:guid}}/replies";
        public const string Delete = GetById;
    }

    public static class Reactions
    {
        public const string ReactOnEntity = $"{ApiBase}/reactions/{{resourceType}}/{{entityId:guid}}";
        public const string ListEntityReactions = $"{ApiBase}/{{resourceType}}/{{entityId:guid}}/reactions";
    }

    public static class Follows
    {
        public const string FollowEntity = $"{FollowsBase}/{{resourceType}}/{{entityId:guid}}";
    }

    public static class PlaybackSessions
    {
        public const string ListPlaybackSessionHistory = PlaybackSessionsBase;
        public const string GetOrCreatePlaybackSession = PlaybackSessionsBase;
        public const string MarkPlaybackProgress = $"{PlaybackSessionsBase}/{{sessionId}}/progress";
    }
}