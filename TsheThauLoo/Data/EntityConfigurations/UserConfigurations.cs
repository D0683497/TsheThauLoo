using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Data.EntityConfigurations;

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
            b.Property(x => x.Id).HasMaxLength(25);

            b.Property(x => x.SecurityStamp).HasMaxLength(25).IsRequired();

            b.Property(x => x.ConcurrencyStamp).HasMaxLength(25).IsRequired();

            b.Property(x => x.Email).IsRequired();

            b.Property(x => x.NormalizedEmail).IsRequired();

            b.Property(x => x.PhoneNumber).HasMaxLength(30);

            b.HasOne(x => x.Alumnus).WithOne(a => a.User);

            b.HasOne(x => x.Employee).WithOne(e => e.User);

            b.HasOne(x => x.Staff).WithOne(s => s.User);

            b.HasOne(x => x.Student).WithOne(s => s.User);
        });

        builder.Entity<ApplicationUserClaim>(b =>
        {
            b.Property(x => x.UserId).HasMaxLength(25);

            b.Property(x => x.ClaimType).IsRequired();

            b.Property(x => x.ClaimValue).IsRequired();
        });

        builder.Entity<ApplicationRole>(b =>
        {
            b.Property(x => x.Id).HasMaxLength(25);

            b.Property(x => x.Name).IsRequired();

            b.Property(x => x.NormalizedName).IsRequired();
        });

        builder.Entity<ApplicationRoleClaim>(b =>
        {
            b.Property(x => x.RoleId).HasMaxLength(25);

            b.Property(x => x.ClaimType).IsRequired();

            b.Property(x => x.ClaimValue).IsRequired();
        });

        builder.Entity<ApplicationUserRole>(b =>
        {
            b.Property(x => x.UserId).HasMaxLength(25);

            b.Property(x => x.RoleId).HasMaxLength(25);
        });

        builder.Entity<ApplicationUserLogin>()
            .Property(b => b.UserId)
            .HasMaxLength(25);

        builder.Entity<ApplicationUserToken>()
            .Property(b => b.UserId)
            .HasMaxLength(25);
    }

    public static void Initialize(ModelBuilder builder)
    {
        var admin = new ApplicationRole("eHEKw9koyc1UahKDJmN2kaYu4", "Admin", "zgOFXPEuCBj0oYx7wcVOxPjuT");
        var alumnus = new ApplicationRole("yu0ZAukYcdagLeUttlGA2dx0i", "Alumnus", "BHdn3idxWd6CxVJbUiZdFizIG");
        var employee = new ApplicationRole("iVslkwN8bQq58oq6Xh7BUT8mE", "Employee", "vm2O6QewMjb46zCjkzfS8WgYX");
        var staff = new ApplicationRole("YLWMXCM7aq6kUbrzBLNBV7edb", "Staff", "9ZOtm2qcp9BU1V45CvjpzbwP0");
        var student = new ApplicationRole("22mrI3gVYA8zWHTw0R0IHPLBy", "Student", "2ptmL3x5IWVQ4DRcV19AS0jfh");
        builder.Entity<ApplicationRole>().HasData(admin, alumnus, employee, staff, student);

        var hasher = new PasswordHasher<ApplicationUser>();
        var user = new ApplicationUser
        {
            UserName = "admin",
            NormalizedUserName = "admin".Replace('\\', '/').ToUpperInvariant(),
            Email = "admin@gmail.com",
            NormalizedEmail = "admin@gmail.com".Replace('\\', '/').ToUpperInvariant(),
            EmailConfirmed = true,
            Name = "管理員"
        };
        user.PasswordHash = hasher.HashPassword(user, "password1234");
        builder.Entity<ApplicationUser>().HasData(user);
        
        builder.Entity<ApplicationUserRole>().HasData(new ApplicationUserRole { UserId = user.Id, RoleId = admin.Id });
    }
}