namespace Audora.Application.Common.Abstractions.Interfaces.Services;

public interface IUserDeletionService
{
    Task<bool> DeleteUserWithChildrenAsync(Guid userId);
}