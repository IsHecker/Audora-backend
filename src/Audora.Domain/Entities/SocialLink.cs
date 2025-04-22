using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class SocialLink : Entity
{
    public Guid CreatorId { get; init; }
    public string PlatformName { get; init; } = null!;
    public string Url { get; init; } = null!;

    public SocialLink(Guid creatorId, string platformName, string url)
    {
        CreatorId = creatorId;
        PlatformName = platformName;
        Url = url;
    }

    private SocialLink()
    {
    }
}