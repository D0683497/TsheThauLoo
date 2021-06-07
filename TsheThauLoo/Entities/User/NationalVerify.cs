using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 實名驗證資料
    /// </summary>
    public class NationalVerify
    {
        /// <summary>
        /// 實名驗證資料識別碼
        /// </summary>
        [Key]
        public string NationalVerifyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 說明
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<NationalVerifyFile> NationalVerifyFiles { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}