namespace Audora.Contracts.Podcasts.Responses;

public class PodcastResponse
{
    public bool IsFollowed { get; init; }
    public byte? UserRating { get; init; }
    public decimal AverageRating { get; init; }
    public int TotalRatings { get; init; }
}