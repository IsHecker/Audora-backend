using Audora.Application.Common.Models;
using Audora.Contracts.Users.Responses;

namespace Audora.Application.Common.Mappings;

public static class UserMapping
{
    public static UserResponse ToResponse(this User user)
    {
        return new UserResponse
        {
            Id = user.Id,
            Name = user.Name,
            AvatarUrl = user.AvatarUrl
        };
    }

    public static IEnumerable<UserResponse> ToResponse(this IEnumerable<User> users)
    {
        return users.Select(ToResponse);
    }
}