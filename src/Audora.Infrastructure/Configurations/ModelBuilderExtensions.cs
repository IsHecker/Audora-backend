using Audora.Infrastructure.Identity;
using Microsoft.EntityFrameworkCore;

namespace Audora.Infrastructure.Configurations;

public static class ModelBuilderExtensions
{
    public static void ApplyUserIdConvention(this ModelBuilder modelBuilder)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            var properties = entityType.ClrType.GetProperties()
                .Where(p => p.Name == "ListenerId" || p.Name == "CreatorId");

            foreach (var prop in properties)
            {
                modelBuilder.Entity(entityType.ClrType)
                    .HasOne(typeof(ApplicationUser)) // or your custom User type
                    .WithMany()
                    .HasForeignKey(prop.Name)
                    .OnDelete(DeleteBehavior.Restrict); // or your preferred rule
            }
        }
    }
}