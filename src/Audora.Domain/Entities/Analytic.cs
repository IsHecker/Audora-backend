using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Analytic : Entity
{
    public Guid CreatorId { get; init; }
    public long PlayCount { get; init; }
    public int Followers { get; init; }
    public long TotalListeningHours { get; init; }

    public Analytic(Guid creatorId, long playCount, int followers, long totalListeningHours)
    {
        CreatorId = creatorId;
        PlayCount = playCount;
        Followers = followers;
        TotalListeningHours = totalListeningHours;
    }

    private Analytic()
    {
    }
}