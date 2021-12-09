using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 企業使用者
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 職稱
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Title { get; set; } = null!;

        /// <summary>
        /// 部門
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Division { get; set; } = null!;

        /// <summary>
        /// 職務代理人
        /// </summary>
        public Substitute Substitute { get; set; } = null!;

        [Required]
        [MaxLength(36)]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
