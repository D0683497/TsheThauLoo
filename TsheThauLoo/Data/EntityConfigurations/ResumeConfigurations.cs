using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.Resume;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class ResumeConfigurations
    {
        public static void ResumeRelation(ModelBuilder builder)
        {
            #region ApplicationUser 跟 FileResume 一對多

            builder.Entity<FileResume>()
                .HasOne(resume => resume.ApplicationUser)
                .WithMany(user => user.FileResumes)
                .HasForeignKey(resume => resume.ApplicationUserId);

            #endregion
        }
    }
}