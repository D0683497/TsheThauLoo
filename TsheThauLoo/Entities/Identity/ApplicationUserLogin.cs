using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity;

public class ApplicationUserLogin : IdentityUserLogin<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}