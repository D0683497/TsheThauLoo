using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 系列活動附檔
    /// </summary>
    public class CampaignFile : Document
    {
        /// <summary>
        /// 系列活動附檔識別碼
        /// </summary>
        [Key]
        public string CampaignFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string CampaignId { get; set; }

        public Campaign Campaign { get; set; }
    }
}