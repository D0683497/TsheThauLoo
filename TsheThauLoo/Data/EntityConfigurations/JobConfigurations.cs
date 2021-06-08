using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Job;
using TsheThauLoo.Entities.Resume;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class JobConfigurations
    {
        public static void JobRelation(ModelBuilder builder)
        {
            #region RecruitmentCampaign 跟 RecruitmentCampaignOpening 一對多

            builder.Entity<RecruitmentCampaignOpening>()
                .HasOne(opening => opening.RecruitmentCampaign)
                .WithMany(recruitment => recruitment.RecruitmentCampaignOpenings)
                .HasForeignKey(opening => opening.RecruitmentCampaignId);

            #endregion
            
            #region RecruitmentCampaignOpening 跟 RecruitmentCampaignResume 一對多

            builder.Entity<RecruitmentCampaignResume>()
                .HasOne(resume => resume.RecruitmentCampaignOpening)
                .WithMany(opening => opening.RecruitmentCampaignResumes)
                .HasForeignKey(resume => resume.RecruitmentCampaignOpeningId);

            #endregion
            
            #region Examiner 跟 RecruitmentCampaignResume 一對多

            builder.Entity<RecruitmentCampaignResume>()
                .HasOne(resume => resume.Examiner)
                .WithMany(examiner => examiner.RecruitmentCampaignResumes)
                .HasForeignKey(resume => resume.ExaminerId);

            #endregion
            
            #region FileResume 跟 RecruitmentCampaignResume 一對一

            builder.Entity<FileResume>()
                .HasOne(file => file.RecruitmentCampaignResume)
                .WithOne(recruitment => recruitment.FileResume)
                .HasForeignKey<RecruitmentCampaignResume>(recruitment => recruitment.FileResumeId);

            #endregion
            
            #region RecruitmentCampaignOpening 跟 Qualification 一對多

            builder.Entity<Qualification>()
                .HasOne(qualification => qualification.RecruitmentCampaignOpening)
                .WithMany(opening => opening.Qualifications)
                .HasForeignKey(qualification => qualification.RecruitmentCampaignOpeningId);

            #endregion
            
            #region RecruitmentCampaignOpening 跟 Faculty 一對多

            builder.Entity<Faculty>()
                .HasOne(faculty => faculty.RecruitmentCampaignOpening)
                .WithMany(opening => opening.Faculties)
                .HasForeignKey(faculty => faculty.RecruitmentCampaignOpeningId);

            #endregion
        }
    }
}