using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Invitation : Entity
{
    public Guid PodcastId { get; init; }
    public string InvitedEmail { get; init; } = null!;
    public Guid InvitedById { get; init; }
    public long ExpiresAt { get; init; }


    public Invitation(
        Guid podcastId,
        string invitedEmail,
        Guid invitedById,
        long expiresAt)
    {
        PodcastId = podcastId;
        InvitedEmail = invitedEmail;
        InvitedById = invitedById;
        ExpiresAt = expiresAt;
    }

    private Invitation()
    {
    }
}