using Audora.Application.Common.Abstractions.Interfaces;
using Audora.Application.Common.Models;

namespace Audora.Infrastructure.Services;

public class UserService : IUserService
{
    public async Task<IQueryable<User>> GetUsersAsync()
    {
        throw new NotImplementedException();
    }

    public async Task<IQueryable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds)
    {
        throw new NotImplementedException();
    }
}