using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = null!;
    }
}
