using Microsoft.EntityFrameworkCore;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Data.EntityConfigurations
{
    public static class UserConfigurations
    {
        public static void UserRelation(ModelBuilder builder)
        {
            #region ApplicationUser

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.UserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.NormalizedUserName)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.Email)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.NormalizedEmail)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.PhoneNumber)
                .IsUnique();

            builder.Entity<ApplicationUser>()
                .HasIndex(x => x.NationalId)
                .IsUnique();

            #endregion

            #region NetworkId

            builder.Entity<Administrator>()
                .HasIndex(x => x.NetworkId)
                .IsUnique();
            
            builder.Entity<Employee>()
                .HasIndex(x => x.NetworkId)
                .IsUnique();
            
            builder.Entity<Student>()
                .HasIndex(x => x.NetworkId)
                .IsUnique();

            #endregion
            
            #region ApplicationUser 跟 UserPhoto 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.UserPhoto)
                .WithOne(photo => photo.ApplicationUser)
                .HasForeignKey<UserPhoto>(photo => photo.ApplicationUserId);

            #endregion
            
            #region ApplicationUser 跟 NationalVerify 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.NationalVerify)
                .WithOne(verify => verify.ApplicationUser)
                .HasForeignKey<NationalVerify>(verify => verify.ApplicationUserId);

            #endregion
            
            #region NationalVerify 跟 NationalVerifyFile 一對多

            builder.Entity<NationalVerifyFile>()
                .HasOne(file => file.NationalVerify)
                .WithMany(verify => verify.NationalVerifyFiles)
                .HasForeignKey(file => file.NationalVerifyId);

            #endregion
            
            #region ApplicationUser 跟 Administrator 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Administrator)
                .WithOne(administrator => administrator.ApplicationUser)
                .HasForeignKey<Administrator>(administrator => administrator.ApplicationUserId);

            #endregion
            
            #region Administrator 跟 Responsibility 一對多

            builder.Entity<Responsibility>()
                .HasOne(responsibility => responsibility.Administrator)
                .WithMany(administrator => administrator.Responsibilities)
                .HasForeignKey(responsibility => responsibility.AdministratorId);

            #endregion
            
            #region ApplicationUser 跟 Alumnus 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Alumnus)
                .WithOne(alumnus => alumnus.ApplicationUser)
                .HasForeignKey<Alumnus>(alumnus => alumnus.ApplicationUserId);

            #endregion
            
            #region Alumnus 跟 AlumnusVerify 一對一

            builder.Entity<Alumnus>()
                .HasOne(alumnus => alumnus.AlumnusVerify)
                .WithOne(verify => verify.Alumnus)
                .HasForeignKey<AlumnusVerify>(verify => verify.AlumnusId);

            #endregion
            
            #region AlumnusVerify 跟 AlumnusVerifyFile 一對多

            builder.Entity<AlumnusVerifyFile>()
                .HasOne(file => file.AlumnusVerify)
                .WithMany(verify => verify.AlumnusVerifyFiles)
                .HasForeignKey(file => file.AlumnusVerifyId);

            #endregion
            
            #region ApplicationUser 跟 Employee 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Employee)
                .WithOne(employee => employee.ApplicationUser)
                .HasForeignKey<Employee>(employee => employee.ApplicationUserId);

            #endregion
            
            #region ApplicationUser 跟 Examiner 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Examiner)
                .WithOne(examiner => examiner.ApplicationUser)
                .HasForeignKey<Examiner>(examiner => examiner.ApplicationUserId);

            #endregion
            
            #region ApplicationUser 跟 Manager 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Manager)
                .WithOne(manager => manager.ApplicationUser)
                .HasForeignKey<Manager>(manager => manager.ApplicationUserId);

            #endregion
            
            #region Manager 跟 Substitute 一對一

            builder.Entity<Manager>()
                .HasOne(manager => manager.Substitute)
                .WithOne(substitute => substitute.Manager)
                .HasForeignKey<Substitute>(substitute => substitute.ManagerId);

            #endregion
            
            #region ApplicationUser 跟 Student 一對一

            builder.Entity<ApplicationUser>()
                .HasOne(user => user.Student)
                .WithOne(student => student.ApplicationUser)
                .HasForeignKey<Student>(student => student.ApplicationUserId);

            #endregion
            
            #region Student 跟 StudentVerify 一對一

            builder.Entity<Student>()
                .HasOne(student => student.StudentVerify)
                .WithOne(verify => verify.Student)
                .HasForeignKey<StudentVerify>(verify => verify.StudentId);

            #endregion
            
            #region StudentVerify 跟 StudentVerifyFile 一對多

            builder.Entity<StudentVerifyFile>()
                .HasOne(file => file.StudentVerify)
                .WithMany(verify => verify.StudentVerifyFiles)
                .HasForeignKey(file => file.StudentVerifyId);

            #endregion
        }
    }
}