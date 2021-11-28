using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 在校生驗證資料
    /// </summary>
    public class StudentVerify
    {
        /// <summary>
        /// 在校生驗證資料識別碼
        /// </summary>
        [Key]
        public string StudentVerifyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 說明
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<StudentVerifyFile> StudentVerifyFiles { get; set; }
        
        public string StudentId { get; set; }

        public Student Student { get; set; }
    }
}