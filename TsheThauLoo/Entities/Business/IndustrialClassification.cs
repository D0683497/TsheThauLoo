using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Business
{
    /// <summary>
    /// 產業類別
    /// </summary>
    public class IndustrialClassification
    {
        /// <summary>
        /// 產業類別識別碼
        /// </summary>
        [Key]
        public string IndustrialClassificationId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 說明
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Description { get; set; }
        
        public string CompanyId { get; set; }

        public Company Company { get; set; }
    }
}