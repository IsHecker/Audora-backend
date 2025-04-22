using Audora.Domain.Common;

namespace Audora.Domain.Entities;

public class Notification : Entity
{
    public Guid UserId { get; init; }
    public string Message { get; init; } = null!;
    public NotificationType NotificationType { get; init; }     //(e.g., comment alert, subscriber milestone, revenue update)
    public bool IsSeen { get; init; }
    

    public Notification(Guid userId, string message, NotificationType notificationType, bool isSeen)
    {
        UserId = userId;
        Message = message;
        NotificationType = notificationType;
        IsSeen = isSeen;
    }

    private Notification()
    {
    }
}

public enum NotificationType 
{
    Comment,
    React,
    Subscribe,
}