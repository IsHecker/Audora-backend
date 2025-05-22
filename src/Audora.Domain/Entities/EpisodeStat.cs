using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class EpisodeStat : Entity
{
    public Guid PodcastId { get; init; }
    public Guid EpisodeId { get; init; }

    public string EpisodeName { get; private set; } = null!;
    public long ListeningTime { get; private set; }
    public int PlayCount { get; private set; }
    public int Shares { get; private set; }
    public int Bookmarks { get; private set; }
    public int Replays { get; private set; }
    public int Downloads { get; private set; }


    public Episode Episode { get; init; } = null!;

    public EpisodeStat(
        Guid episodeId,
        Guid podcastId,
        string episodeName,
        int shares = 0,
        int bookmarks = 0,
        int listeningTime = 0,
        int playCount = 0,
        int replays = 0,
        int downloads = 0)
    {
        EpisodeId = episodeId;
        PodcastId = podcastId;
        EpisodeName = episodeName;
        Shares = shares;
        Bookmarks = bookmarks;
        ListeningTime = listeningTime;
        PlayCount = playCount;
        Replays = replays;
        Downloads = downloads;
    }

    private EpisodeStat()
    {
    }

    public void UpdateEpisodeName(string newEpisodeName) => EpisodeName = newEpisodeName;

    public void IncreaseDownloadCount() => Downloads++;

    public void IncreasePlayCount() => PlayCount++;

    public void IncreaseReplayCount() => Replays++;

    public void IncreaseShareCount() => Shares++;

    public void IncreaseBookmarkCount() => Bookmarks++;

    public void IncreaseListeningTime(long listenTime) => ListeningTime += listenTime;
}