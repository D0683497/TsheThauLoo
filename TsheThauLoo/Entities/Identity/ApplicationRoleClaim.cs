using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity;

public class ApplicationRoleClaim : IdentityRoleClaim<string>
{
    public virtual ApplicationRole Role { get; set; } = null!;
}