using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Subscription : Entity
{
    public Guid ListenerId { get; init; }
    public SubscriptionPlan SubscriptionPlan { get; init; }
    public long StartedAt { get; init; }
    public long ExpiresAt { get; init; }
    public bool IsActive => DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() > ExpiresAt;


    public Subscription(Guid listenerId, SubscriptionPlan subscriptionPlan, long startedAt, long expiresAt)
    {
        ListenerId = listenerId;
        SubscriptionPlan = subscriptionPlan;
        StartedAt = startedAt;
        ExpiresAt = expiresAt;
    }

    private Subscription()
    {
    }
}

public enum SubscriptionPlan
{
    Free,
    Premium
}