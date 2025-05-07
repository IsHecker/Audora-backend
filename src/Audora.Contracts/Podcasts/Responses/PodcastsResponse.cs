namespace Audora.Contracts.Podcasts.Responses;

public class PodcastsResponse
{
    public IEnumerable<PodcastResponse> Podcasts { get; set; } = null!;
}