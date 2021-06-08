using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class CompanyConfigurations
    {
        public static void CompanyRelation(ModelBuilder builder)
        {
            #region Company 跟 IndustrialClassification 一對多

            builder.Entity<IndustrialClassification>()
                .HasOne(i => i.Company)
                .WithMany(company => company.IndustrialClassifications)
                .HasForeignKey(i => i.CompanyId);

            #endregion
            
            #region Company 跟 CompanyLogo 一對一

            builder.Entity<Company>()
                .HasOne(company => company.CompanyLogo)
                .WithOne(logo => logo.Company)
                .HasForeignKey<CompanyLogo>(logo => logo.CompanyId);

            #endregion
            
            #region Company 跟 CompanyVerify 一對一

            builder.Entity<Company>()
                .HasOne(company => company.CompanyVerify)
                .WithOne(verify => verify.Company)
                .HasForeignKey<CompanyVerify>(verify => verify.CompanyId);

            #endregion
            
            #region CompanyVerify 跟 CompanyVerifyFile 一對多

            builder.Entity<CompanyVerifyFile>()
                .HasOne(file => file.CompanyVerify)
                .WithMany(verify => verify.CompanyVerifyFiles)
                .HasForeignKey(i => i.CompanyVerifyId);

            #endregion
            
            #region Company 跟 Manager 一對多

            builder.Entity<Manager>()
                .HasOne(manager => manager.Company)
                .WithMany(company => company.Managers)
                .HasForeignKey(manager => manager.CompanyId);

            #endregion
            
            #region Company 跟 GeneralCampaign 一對多

            builder.Entity<GeneralCampaign>()
                .HasOne(general => general.Company)
                .WithMany(company => company.GeneralCampaigns)
                .HasForeignKey(general => general.CompanyId);

            #endregion
            
            #region Company 跟 RecruitmentCampaign 一對多

            builder.Entity<RecruitmentCampaign>()
                .HasOne(recruitment => recruitment.Company)
                .WithMany(company => company.RecruitmentCampaigns)
                .HasForeignKey(recruitment => recruitment.CompanyId);

            #endregion
        }
    }
}