namespace Audora.Api;

public static class ApiEndpoints
{
    private const string ApiBase = "api";
    private const string PodcastsBase = $"{ApiBase}/podcasts";
    private const string EpisodesBase = $"{ApiBase}/podcasts";

    public static class Podcasts
    {
        public const string List = PodcastsBase; // Get a paginated list of podcasts (with filters like category, tags, creator, etc).
        public const string GetById = $"{PodcastsBase}/{{podcastId:guid}}";
        public const string GetBySlug = $"{PodcastsBase}/slug/{{slug}}";
        public const string ListPodcastEpisodes = $"{GetById}/episodes"; // List episodes in a specific podcast.
        public const string GetStats = $"{GetById}/stats";
        
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
        
        public const string Create = EpisodesBase;
        public const string Update = $"{GetById}";
        public const string Delete = $"{GetById}";
    }
}