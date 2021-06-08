using System;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.User;

namespace TsheThauLoo.Entities.Resume
{
    /// <summary>
    /// 檔案履歷
    /// </summary>
    public class FileResume : Document
    {
        /// <summary>
        /// 檔案履歷識別碼
        /// </summary>
        [Key]
        public string FileResumeId { get; set; } = Guid.NewGuid().ToString();
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}