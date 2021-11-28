using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 在校生
    /// </summary>
    public class Student
    {
        /// <summary>
        /// 在校生識別碼
        /// </summary>
        [Key]
        public string StudentId { get; set; } =  Guid.NewGuid().ToString();

        /// <summary>
        /// 在校生驗證
        /// </summary>
        [Required]
        public bool StudentConfirmed { get; set; } = false;

        /// <summary>
        /// 學號
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string NetworkId { get; set; }

        /// <summary>
        /// 學院
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string College { get; set; }

        /// <summary>
        /// 系所
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Department { get; set; }

        /// <summary>
        /// 班級
        /// </summary>
        [Required]
        [MaxLength(20)]
        public string Class { get; set; }

        public StudentVerify StudentVerify { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}