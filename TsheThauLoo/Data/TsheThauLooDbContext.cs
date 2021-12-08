using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data.EntityConfigurations;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Data
{
    public class TsheThauLooDbContext 
        : IdentityDbContext<
            ApplicationUser, ApplicationRole, string, 
            ApplicationUserClaim, ApplicationUserRole, ApplicationUserLogin, 
            ApplicationRoleClaim, ApplicationUserToken>
    {
        public TsheThauLooDbContext(DbContextOptions<TsheThauLooDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            UserConfigurations.Relation(builder);
        }
    }
}
