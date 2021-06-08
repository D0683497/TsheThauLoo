using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 教職員工
    /// </summary>
    public class Employee
    {
        /// <summary>
        /// 教職員工識別碼
        /// </summary>
        [Key]
        public string EmployeeId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 教職員工驗證
        /// </summary>
        [Required]
        public bool EmployeeConfirmed { get; set; } = false;

        /// <summary>
        /// 證號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string NetworkId { get; set; }
        
        /// <summary>
        /// 部門(學院)
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Dept { get; set; }

        /// <summary>
        /// 單位(系所)
        /// </summary>
        [MaxLength(20)]
        public string Unit { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}