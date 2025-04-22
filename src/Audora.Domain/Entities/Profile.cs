namespace Audora.Domain.Entities;

public class Profile
{
    public Guid CreatorId { get; init; }
    public string Name { get; init; } = null!;
    public string? Bio { get; init; }
    public string? AvatarUrl { get; init; }

    public Profile(Guid creatorId, string name, string? bio = null, string? avatarUrl = null)
    {
        CreatorId = creatorId;
        Bio = bio;
        AvatarUrl = avatarUrl;
        Name = name;
    }

    private Profile()
    {
    }
}