using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Entities.Identity;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 在校生
    /// </summary>
    public class Student
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 學號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string NetworkId { get; set; } = null!;

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
        [MaxLength(36)]
        public string UserId { get; set; } = null!;

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } = null!;
    }
}
