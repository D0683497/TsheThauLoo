using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 校友
    /// </summary>
    public class Alumnus
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(25)]
        public string Id { get; set; } = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);

        /// <summary>
        /// 畢業年度
        /// </summary>
        /// <remarks>yyyy/MM or yyyy</remarks>
        [Required]
        [MaxLength(7)]
        public string DateOfGraduation { get; set; } = null!;

        /// <summary>
        /// 學院
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string College { get; set; } = null!;

        /// <summary>
        /// 系所
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Department { get; set; } = null!;

        [Required]
        [MaxLength(25)]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
