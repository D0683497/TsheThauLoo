using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Job
{
    /// <summary>
    /// 需求科系
    /// </summary>
    public class Faculty
    {
        /// <summary>
        /// 需求科系識別碼
        /// </summary>
        [Key]
        public string FacultyId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Description  { get; set; }
        
        public string RecruitmentCampaignOpeningId { get; set; }

        public RecruitmentCampaignOpening RecruitmentCampaignOpening { get; set; }
    }
}