using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity;

public class ApplicationUserToken : IdentityUserToken<string>
{
    public virtual ApplicationUser User { get; set; } = null!;
}