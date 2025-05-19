using Audora.Contracts.Analytics.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class EpisodeAnalyticsMapping
{
    public static EpisodeAnalyticsResponse ToResponse(this EpisodeStat episodeStat,
        EngagementStat? engagementStat)
    {
        return new EpisodeAnalyticsResponse
        {
            Likes = engagementStat?.Likes ?? 0,
            Dislikes = engagementStat?.Dislikes ?? 0,
            Comments = engagementStat?.Comments ?? 0,
            PlayCount = episodeStat.PlayCount,
            Downloads = episodeStat.Downloads,
            EpisodeName = episodeStat.EpisodeName,
            ListeningTime = episodeStat.ListeningTime,
            Replays = episodeStat.Replays,
            Bookmarks = episodeStat.Bookmarks,
            Shares = episodeStat.Shares
        };
    }
}