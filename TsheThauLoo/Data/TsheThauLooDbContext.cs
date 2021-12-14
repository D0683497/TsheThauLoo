using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data.EntityConfigurations;
using TsheThauLoo.Entities.Identity;
using TsheThauLoo.Entities.School;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Data;

public class TsheThauLooDbContext 
    : IdentityDbContext<
        ApplicationUser, ApplicationRole, string, 
        ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, 
        ApplicationRoleClaim, ApplicationUserToken>
{
    public TsheThauLooDbContext(DbContextOptions<TsheThauLooDbContext> options) : base(options)
    {

    }

    #region User

    public DbSet<Alumnus> Alumni { get; set; } = null!;

    public DbSet<Employee> Employees { get; set; } = null!;

    public DbSet<Staff> Staffs { get; set; } = null!;

    public DbSet<Student> Students { get; set; } = null!;

    #endregion

    #region School

    public DbSet<College> Colleges { get; set; } = null!;

    public DbSet<Department> Departments { get; set; } = null!;

    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        UserConfigurations.Relation(builder);

        UserConfigurations.Initialize(builder);

        SchoolConfigurations.Relation(builder);

        SchoolConfigurations.Initialize(builder);
    }
}