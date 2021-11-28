using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using TsheThauLoo.Entities.Activity;
using TsheThauLoo.Entities.Resume;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 使用者識別碼
        /// </summary>
        [Key]
        [PersonalData]
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 使用者名稱
        /// </summary>
        [Required]
        [MaxLength(100)]
        [ProtectedPersonalData]
        public override string UserName { get; set; }

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [Required]
        [MaxLength(100)]
        public override string NormalizedUserName { get; set; }

        /// <summary>
        /// 電子郵件
        /// </summary>
        [Required]
        [MaxLength(320)]
        [ProtectedPersonalData]
        public override string Email { get; set; }
        
        /// <summary>
        /// Gets or sets the normalized email address for this user.
        /// </summary>
        [Required]
        [MaxLength(320)]
        public override string NormalizedEmail { get; set; }

        /// <summary>
        /// 電子郵件驗證
        /// </summary>
        [Required]
        [PersonalData]
        public override bool EmailConfirmed { get; set; } = false;

        /// <summary>
        /// Gets or sets a salted and hashed representation of the password for this user.
        /// </summary>
        public override string PasswordHash { get; set; }
        
        /// <summary>
        /// A random value that must change whenever a users credentials change (password changed, login removed)
        /// </summary>
        public override string SecurityStamp { get; set; }
        
        /// <summary>
        /// A random value that must change whenever a user is persisted to the store
        /// </summary>
        [ConcurrencyCheck]
        public override string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 手機號碼
        /// </summary>
        [MaxLength(30)]
        [ProtectedPersonalData]
        public override string PhoneNumber { get; set; }

        /// <summary>
        /// 手機號碼驗證
        /// </summary>
        [Required]
        [PersonalData]
        public override bool PhoneNumberConfirmed { get; set; } = false;

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        [Required]
        [PersonalData]
        public override bool TwoFactorEnabled { get; set; } = false;
        
        /// <summary>
        /// Gets or sets the date and time, in UTC, when any user lockout ends.
        /// </summary>
        /// <remarks>
        /// A value in the past means the user is not locked out.
        /// </remarks>
        public override DateTimeOffset? LockoutEnd { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public override bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the number of failed login attempts for the current user.
        /// </summary>
        public override int AccessFailedCount { get; set; } = 0;
        
        /// <summary>
        /// 啟用帳戶
        /// </summary>
        [Required]
        [PersonalData]
        public bool IsEnable { get; set; } = false;
        
        /// <summary>
        /// 實名驗證
        /// </summary>
        [Required]
        [PersonalData]
        public bool IdentityConfirmed { get; set; } = false;
        
        /// <summary>
        /// 身分證字號
        /// </summary>
        [MaxLength(15)]
        [ProtectedPersonalData]
        public string NationalId { get; set; }
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        [ProtectedPersonalData]
        public string Name { get; set; }
        
        /// <summary>
        /// 性別
        /// </summary>
        [ProtectedPersonalData]
        public GenderType? Gender { get; set; }
        
        /// <summary>
        /// 生日
        /// </summary>
        [ProtectedPersonalData]
        public DateTime? DateOfBirth { get; set; }
        
        /// <summary>
        /// 通訊地址
        /// </summary>
        [MaxLength(200)]
        [ProtectedPersonalData]
        public string CurrentAddress { get; set; }

        public UserPhoto UserPhoto { get; set; }

        public NationalVerify NationalVerify { get; set; }

        public Administrator Administrator { get; set; }

        public Alumnus Alumnus { get; set; }

        public Employee Employee { get; set; }

        public Examiner Examiner { get; set; }

        public Manager Manager { get; set; }

        public Student Student { get; set; }

        public ICollection<EventAttendee> EventAttendees { get; set; }

        public ICollection<GeneralCampaignAttendee> GeneralCampaignAttendees { get; set; }

        public ICollection<FileResume> FileResumes { get; set; }
    }
}