using Microsoft.AspNetCore.Identity;

namespace Audora.Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FullName { get; set; } = null!;
    public string? PictureUrl { get; set; }
}