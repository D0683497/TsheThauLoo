using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 教職員工
    /// </summary>
    public class Staff
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(25)]
        public string Id { get; set; } = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);

        /// <summary>
        /// 證號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string NetworkId { get; set; } = null!;

        /// <summary>
        /// 部門(學院)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Dept { get; set; } = null!;

        /// <summary>
        /// 單位(系所)
        /// </summary>
        [MaxLength(30)]
        public string? Unit { get; set; } = null;

        [Required]
        [MaxLength(25)]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
