using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 校友驗證附檔
    /// </summary>
    public class AlumnusVerifyFile : Document
    {
        /// <summary>
        /// 校友驗證附檔識別碼
        /// </summary>
        [Key]
        public string AlumnusVerifyFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string AlumnusVerifyId { get; set; }

        public AlumnusVerify AlumnusVerify { get; set; }
    }
}