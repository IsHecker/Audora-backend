using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Models;

namespace Audora.Infrastructure.Services;

public class UserService : IUserService
{
    readonly IQueryable<User> users = new List<User>
    {
        new() { Id = Guid.NewGuid(), Name = "Alice Johnson", AvatarUrl = "https://example.com/avatars/alice.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Bob Smith", AvatarUrl = "https://example.com/avatars/bob.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Charlie Brown", AvatarUrl = "https://example.com/avatars/charlie.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Diana Prince", AvatarUrl = "https://example.com/avatars/diana.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Ethan Hunt", AvatarUrl = "https://example.com/avatars/ethan.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Fiona Gallagher", AvatarUrl = "https://example.com/avatars/fiona.jpg" },
        new() { Id = Guid.NewGuid(), Name = "George Clooney", AvatarUrl = "https://example.com/avatars/george.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Hannah Montana", AvatarUrl = "https://example.com/avatars/hannah.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Ian Somerhalder", AvatarUrl = "https://example.com/avatars/ian.jpg" },
        new() { Id = Guid.NewGuid(), Name = "Jasmine Lee", AvatarUrl = "https://example.com/avatars/jasmine.jpg" }
    }.AsQueryable();


    public async Task<IQueryable<User>> GetUsersAsync()
    {
        return users;
    }

    public async Task<IQueryable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
    {
        return users.Where(u => userIds.Contains(u.Id));
    }
}