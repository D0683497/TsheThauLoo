using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.Business;
using TsheThauLoo.Entities.Job;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 徵才子活動
    /// </summary>
    public class RecruitmentCampaign
    {
        /// <summary>
        /// 徵才子活動識別碼
        /// </summary>
        [Key]
        public string RecruitmentCampaignId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 名稱
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Title { get; set; }
        
        /// <summary>
        /// 內容
        /// </summary>
        [Required]
        public string Content { get; set; }
        
        /// <summary>
        /// 開始時間
        /// </summary>
        [Required]
        public DateTime StartTime { get; set; }
        
        /// <summary>
        /// 結束時間
        /// </summary>
        [Required]
        public DateTime EndTime { get; set; }

        /// <summary>
        /// 啟用審查
        /// </summary>
        [Required]
        public bool EnableReview { get; set; } = false;

        public ICollection<RecruitmentCampaignFile> RecruitmentCampaignFiles { get; set; }
        
        public ICollection<RecruitmentCampaignOpening> RecruitmentCampaignOpenings { get; set; }

        public string CompanyId { get; set; }

        public Company Company { get; set; }

        public string CampaignId { get; set; }

        public Campaign Campaign { get; set; }
    }
}