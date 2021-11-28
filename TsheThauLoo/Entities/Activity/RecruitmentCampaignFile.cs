using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 徵才子活動附檔
    /// </summary>
    public class RecruitmentCampaignFile : Document
    {
        /// <summary>
        /// 徵才子活動附檔識別碼
        /// </summary>
        [Key]
        public string RecruitmentCampaignFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string RecruitmentCampaignId { get; set; }

        public RecruitmentCampaign RecruitmentCampaign { get; set; }
    }
}