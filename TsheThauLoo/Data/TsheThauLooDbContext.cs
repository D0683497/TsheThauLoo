using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Data.EntityConfigurations;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Data
{
    public class TsheThauLooDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public TsheThauLooDbContext(DbContextOptions<TsheThauLooDbContext> options) : base(options)
        {
            
        }

        #region User

        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<NationalVerify> NationalVerifies { get; set; }
        public DbSet<NationalVerifyFile> NationalVerifyFiles { get; set; }
        public DbSet<Administrator> Administrators { get; set; }
        public DbSet<Responsibility> Responsibilities { get; set; }
        public DbSet<Alumnus> Alumni { get; set; }
        public DbSet<AlumnusVerify> AlumnusVerifies { get; set; }
        public DbSet<AlumnusVerifyFile> AlumnusVerifyFiles { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Examiner> Examiners { get; set; }
        public DbSet<Manager> Managers { get; set; }
        public DbSet<Substitute> Substitutes { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentVerify> StudentVerifies { get; set; }
        public DbSet<StudentVerifyFile> StudentVerifyFiles { get; set; }

        #endregion

        #region Activity

        public DbSet<Event> Events { get; set; }
        public DbSet<EventFile> EventFiles { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }
        public DbSet<EventParticipant> EventParticipants { get; set; }
        public DbSet<Campaign> Campaigns { get; set; }
        public DbSet<CampaignFile> CampaignFiles { get; set; }
        public DbSet<GeneralCampaign> GeneralCampaigns { get; set; }
        public DbSet<GeneralCampaignFile> GeneralCampaignFiles { get; set; }
        public DbSet<GeneralCampaignAttendee> GeneralCampaignAttendees { get; set; }
        public DbSet<GeneralCampaignParticipant> GeneralCampaignParticipants { get; set; }
        public DbSet<RecruitmentCampaign> RecruitmentCampaigns { get; set; }
        public DbSet<RecruitmentCampaignFile> RecruitmentCampaignFiles { get; set; }

        #endregion

        #region Resume

        public DbSet<FileResume> FileResumes { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            
            UserConfigurations.UserRelation(builder);
            ActivityConfigurations.ActivityRelation(builder);
            ResumeConfigurations.ResumeRelation(builder);
        }
    }
}