using Audora.Contracts.Analytics.Responses;
using Audora.Contracts.Reactions.Responses;

namespace Audora.Contracts.Episodes.Responses;

public class EpisodeResponse
{
    public Guid Id { get; init; }
    public string PodcastName { get; init; } = null!;
    public string Name { get; init; } = null!;
    public string Description { get; init; } = null!;
    public string? CoverImageUrl { get; init; }
    public long Duration { get; init; }
    public Guid AudioFileId { get; init; }
    public int EpisodeNumber { get; init; }
    public DateTime ReleaseDate { get; init; }
    public ListenerReactionResponse? ListenerReaction { get; set; }
    public EpisodeAnalyticsResponse? EpisodeStat { get; set; }
}