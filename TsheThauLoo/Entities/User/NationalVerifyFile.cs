using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 實名驗證附檔
    /// </summary>
    public class NationalVerifyFile : Document
    {
        /// <summary>
        /// 實名驗證附檔識別碼
        /// </summary>
        [Key]
        public string NationalVerifyFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string NationalVerifyId { get; set; }

        public NationalVerify NationalVerify { get; set; }
    }
}