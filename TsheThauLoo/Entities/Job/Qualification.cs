using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Job
{
    /// <summary>
    /// 資格條件
    /// </summary>
    public class Qualification
    {
        /// <summary>
        /// 資格條件識別碼
        /// </summary>
        [Key]
        public string QualificationId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Description  { get; set; }
        
        public string RecruitmentCampaignOpeningId { get; set; }

        public RecruitmentCampaignOpening RecruitmentCampaignOpening { get; set; }
    }
}