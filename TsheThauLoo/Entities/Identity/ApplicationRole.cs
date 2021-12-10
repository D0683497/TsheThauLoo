using Microsoft.AspNetCore.Identity;

namespace TsheThauLoo.Entities.Identity
{
    public class ApplicationRole : IdentityRole
    {
        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

        public virtual ICollection<ApplicationRoleClaim> RoleClaims { get; set; } = null!;

        public ApplicationRole()
        {
            Id = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);
            ConcurrencyStamp = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);
        }

        public ApplicationRole(string roleName) : this()
        {
            Name = roleName;
            NormalizedName = roleName.Replace('\\', '/').ToUpperInvariant();
        }
    }
}
