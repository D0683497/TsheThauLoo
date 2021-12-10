using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.School
{
    /// <summary>
    /// 學院
    /// </summary>
    public class College
    {
        /// <summary>
        /// 識別碼
        /// </summary>
        [Key]
        [MaxLength(25)]
        public string Id { get; set; } = Nanoid.Nanoid.Generate("0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ", 25);

        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Name { get; set; } = null!;

        /// <summary>
        /// 系所
        /// </summary>
        public virtual ICollection<Department> Departments { get; set; } = null!;

        public College(string id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
