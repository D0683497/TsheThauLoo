using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities
{
    /// <summary>
    /// 檔案
    /// </summary>
    public class Document
    {
        /// <summary>
        /// 檔案類型
        /// </summary>
        [Required]
        [MaxLength(130)]
        public string Type { get; set; }
        
        /// <summary>
        /// 檔案名稱
        /// </summary>
        [Required]
        [MaxLength(260)]
        public string Name { get; set; }

        /// <summary>
        /// 副檔名
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string Extension { get; set; }
        
        /// <summary>
        /// 檔案路徑
        /// </summary>
        [Required]
        public string Path { get; set; }
    }
}