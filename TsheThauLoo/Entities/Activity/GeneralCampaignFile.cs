using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般子活動附檔
    /// </summary>
    public class GeneralCampaignFile : Document
    {
        /// <summary>
        /// 一般子活動附檔識別碼
        /// </summary>
        [Key]
        public string GeneralCampaignFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string GeneralCampaignId { get; set; }

        public GeneralCampaign GeneralCampaign { get; set; }
    }
}