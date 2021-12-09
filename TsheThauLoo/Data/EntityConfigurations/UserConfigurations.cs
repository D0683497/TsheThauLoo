using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class UserConfigurations
    {
        public static void Relation(ModelBuilder builder)
        {
            builder.Entity<ApplicationUser>(b =>
            {
                // Each User can have many UserClaims
                b.HasMany(e => e.Claims)
                    .WithOne(e => e.User)
                    .HasForeignKey(uc => uc.UserId)
                    .IsRequired();

                // Each User can have many UserLogins
                b.HasMany(e => e.Logins)
                    .WithOne(e => e.User)
                    .HasForeignKey(ul => ul.UserId)
                    .IsRequired();

                // Each User can have many UserTokens
                b.HasMany(e => e.Tokens)
                    .WithOne(e => e.User)
                    .HasForeignKey(ut => ut.UserId)
                    .IsRequired();

                // Each User can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.User)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });

            builder.Entity<ApplicationRole>(b =>
            {
                // Each Role can have many entries in the UserRole join table
                b.HasMany(e => e.UserRoles)
                    .WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                // Each Role can have many associated RoleClaims
                b.HasMany(e => e.RoleClaims)
                    .WithOne(e => e.Role)
                    .HasForeignKey(rc => rc.RoleId)
                    .IsRequired();
            });

            builder.Entity<ApplicationUser>(b =>
            {
                b.Property(x => x.Id)
                    .HasMaxLength(36);

                b.HasOne(b => b.Alumnus)
                    .WithOne(a => a.User)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Employee)
                    .WithOne(e => e.User)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Staff)
                    .WithOne(s => s.User)
                    .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Student)
                    .WithOne(s => s.User)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<ApplicationUserClaim>(b =>
            {
                b.Property(x => x.Id)
                    .HasMaxLength(36);

                b.Property(x => x.UserId)
                    .HasMaxLength(36);
            });

            builder.Entity<ApplicationRole>()
                .Property(b => b.Id)
                .HasMaxLength(36);

            builder.Entity<ApplicationRoleClaim>(b =>
            {
                b.Property(x => x.Id)
                    .HasMaxLength(36);

                b.Property(x => x.RoleId)
                    .HasMaxLength(36);
            });

            builder.Entity<ApplicationUserRole>(b =>
            {
                b.Property(x => x.UserId)
                    .HasMaxLength(36);

                b.Property(x => x.RoleId)
                    .HasMaxLength(36);
            });

            builder.Entity<ApplicationUserLogin>()
                .Property(b => b.UserId)
                .HasMaxLength(36);

            builder.Entity<ApplicationUserToken>()
                .Property(b => b.UserId)
                .HasMaxLength(36);
        }
    }
}
