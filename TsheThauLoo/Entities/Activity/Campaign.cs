using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 系列活動
    /// </summary>
    public class Campaign
    {
        /// <summary>
        /// 系列活動識別碼
        /// </summary>
        [Key]
        public string CampaignId { get; set; } = Guid.NewGuid().ToString();
        
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

        public ICollection<CampaignFile> CampaignFiles { get; set; }
        
        public ICollection<GeneralCampaign> GeneralCampaigns { get; set; }

        public ICollection<RecruitmentCampaign> RecruitmentCampaigns { get; set; }
    }
}