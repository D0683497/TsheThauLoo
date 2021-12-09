using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.School
{
    /// <summary>
    /// 系所
    /// </summary>
    public class Department
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 學位
        /// </summary>
        [Required]
        public DegreeType Degree { get; set; }

        [Required]
        [MaxLength(36)]
        public string CollegeId { get; set; } = null!;

        [ForeignKey("CollegeId")]
        public virtual College College { get; set; } = null!;

        public Department(string id, string name, DegreeType degree, string collegeId)
        {
            Id = id;
            Name = name;
            Degree = degree;
            CollegeId = collegeId;
        }
    }
}
