using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Business
{
    /// <summary>
    /// 公司驗證資料
    /// </summary>
    public class CompanyVerify
    {
        /// <summary>
        /// 公司驗證資料識別碼
        /// </summary>
        [Key]
        public string CompanyVerifyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 說明
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }
        
        public ICollection<CompanyVerifyFile> CompanyVerifyFiles { get; set; }
        
        public string CompanyId { get; set; }

        public Company Company { get; set; }
    }
}