﻿using System.ComponentModel.DataAnnotations;

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
        [MaxLength(36)]
        public string Id { get; set; } = Guid.NewGuid().ToString();

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

        public College(string name)
        {
            Name = name;
        }
    }
}
