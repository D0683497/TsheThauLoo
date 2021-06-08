using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般子活動現場報名者
    /// </summary>
    public class GeneralCampaignParticipant
    {
        /// <summary>
        /// 一般子活動現場報名者識別碼
        /// </summary>
        [Key]
        public string GeneralCampaignParticipantId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 姓名
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        
        /// <summary>
        /// 聯絡用電話號碼
        /// </summary>
        [Required]
        [MaxLength(30)]
        public string ContactPhone { get; set; }
        
        /// <summary>
        /// 備註
        /// </summary>
        [MaxLength(500)]
        public string Remark { get; set; }
        
        public string GeneralCampaignId { get; set; }

        public GeneralCampaign GeneralCampaign { get; set; }
    }
}