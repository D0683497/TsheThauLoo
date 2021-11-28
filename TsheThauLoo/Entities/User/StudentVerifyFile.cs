using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 在校生驗證附檔
    /// </summary>
    public class StudentVerifyFile : Document
    {
        /// <summary>
        /// 在校生驗證附檔識別碼
        /// </summary>
        [Key]
        public string StudentVerifyFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string StudentVerifyId { get; set; }

        public StudentVerify StudentVerify { get; set; }
    }
}