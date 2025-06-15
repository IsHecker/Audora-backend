using Audora.Application.Common.Abstractions.Interfaces.Services;
using Audora.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Services;

public class UserDeletionService : IUserDeletionService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserDeletionService(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    public async Task<bool> DeleteUserWithChildrenAsync(Guid userId)
    {
        // TODO: continue this later.

        var user = await _context.Users.FindAsync(userId);
        if (user is null)
            throw new KeyNotFoundException();

        var userRole = (await _userManager.GetRolesAsync(user!)).First();
        var propertyName = $"{userRole}Id";

        var userReferencedEntities = _context.Model.GetEntityTypes()
            .Where(e => e.ClrType.GetProperties()
                .Any(p => p.Name == propertyName))
            .Select(entity => new
            {
                EntityType = entity,
                Property = entity.ClrType.GetProperties()
                    .FirstOrDefault(p => p.Name == propertyName)
            });


        var setMethod = _context.GetType().GetMethod("Set", Type.EmptyTypes)!;

        foreach (var entity in userReferencedEntities)
        {
            var dbSet = setMethod.MakeGenericMethod(entity.EntityType.ClrType).Invoke(_context, null);
            var queryable = dbSet as IQueryable<object>;
            var toDelete = queryable!
                .Where(e =>
                    (Guid?)entity.Property!.GetValue(e)! == userId
                ).ToList();

            _context.RemoveRange(toDelete);
        }
        var deleteResults = await _userManager.DeleteAsync(user);
        return deleteResults.Succeeded;
    }
}