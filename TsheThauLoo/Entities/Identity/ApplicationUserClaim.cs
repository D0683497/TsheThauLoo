using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity
{
    public class ApplicationUserClaim : IdentityUserClaim<string>
    {
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
