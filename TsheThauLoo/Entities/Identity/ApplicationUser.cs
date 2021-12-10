using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// 身分證字號
        /// </summary>
        [MaxLength(15)]
        public string? NationalId { get; set; } = null;

        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 性別
        /// </summary>
        public GenderType? Gender { get; set; } = null;

        /// <summary>
        /// 生日
        /// </summary>
        /// <remarks>yyyy/MM/dd</remarks>
        [MaxLength(10)]
        public string? DateOfBirth { get; set; } = null;

        /// <summary>
        /// 地址
        /// </summary>
        [MaxLength(256)]
        public string? Address { get; set; } = null;

        /// <summary>
        /// 校友
        /// </summary>
        public virtual Alumnus? Alumnus { get; set; } = null;

        /// <summary>
        /// 企業使用者
        /// </summary>
        public virtual Employee? Employee { get; set; } = null;

        /// <summary>
        /// 教職員工
        /// </summary>
        public virtual Staff? Staff { get; set; } = null;

        /// <summary>
        /// 在校生
        /// </summary>
        public virtual Student? Student { get; set; } = null;

        public virtual ICollection<ApplicationUserClaim> Claims { get; set; } = null!;

        public virtual ICollection<ApplicationUserLogin> Logins { get; set; } = null!;

        public virtual ICollection<ApplicationUserToken> Tokens { get; set; } = null!;

        public virtual ICollection<ApplicationUserRole> UserRoles { get; set; } = null!;

        public ApplicationUser()
        {
            Id = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);
            SecurityStamp = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);
            ConcurrencyStamp = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);
        }
    }
}
