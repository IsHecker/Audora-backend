using Audora.Application.Common.Models;

namespace Audora.Application.Common.Abstractions.Interfaces;

public interface IUserService
{
    Task<IQueryable<User>> GetUsersAsync();
    Task<IQueryable<User>> GetUsersByIdsAsync(IEnumerable<Guid> userIds);
}