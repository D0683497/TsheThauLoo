using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Business
{
    /// <summary>
    /// 公司驗證資料附檔
    /// </summary>
    public class CompanyVerifyFile : Document
    {
        /// <summary>
        /// 公司驗證資料附檔識別碼
        /// </summary>
        [Key]
        public string CompanyVerifyFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string CompanyVerifyId { get; set; }

        public CompanyVerify CompanyVerify { get; set; }
    }
}