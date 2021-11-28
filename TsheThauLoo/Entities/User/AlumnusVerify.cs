using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.User
{
    /// <summary>
    /// 校友驗證資料
    /// </summary>
    public class AlumnusVerify
    {
        /// <summary>
        /// 校友驗證資料識別碼
        /// </summary>
        [Key]
        public string AlumnusVerifyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 說明
        /// </summary>
        [MaxLength(500)]
        public string Description { get; set; }

        public ICollection<AlumnusVerifyFile> AlumnusVerifyFiles { get; set; }
        
        public string AlumnusId { get; set; }

        public Alumnus Alumnus { get; set; }
    }
}