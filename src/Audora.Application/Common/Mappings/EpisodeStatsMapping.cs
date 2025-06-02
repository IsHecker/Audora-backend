using Audora.Contracts.Analytics.Responses;
using Audora.Contracts.EngagementStats.Responses;
using Audora.Domain.Entities;

namespace Audora.Application.Common.Mappings;

public static class EpisodeAnalyticsMapping
{
    public static EpisodeStatsResponse ToResponse(this EpisodeStat episodeStat,
        EngagementStatsResponse? engagementStatResponse = null)
    {
        return new EpisodeStatsResponse
        {
            Comments = engagementStatResponse?.CommentCount ?? 0,
            PlayCount = episodeStat.PlayCount,
            Downloads = episodeStat.Downloads,
            EpisodeName = episodeStat.EpisodeName,
            ListeningTime = episodeStat.ListeningTime,
            Replays = episodeStat.Replays,
            Bookmarks = episodeStat.Bookmarks,
            Shares = episodeStat.Shares,
            Reactions = engagementStatResponse?.Reactions
        };
    }
}