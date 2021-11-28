using System;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般子活動參與者
    /// </summary>
    public class GeneralCampaignAttendee
    {
        /// <summary>
        /// 一般子活動參與者識別碼
        /// </summary>
        [Key]
        public string GeneralCampaignAttendeeId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 參與狀態
        /// </summary>
        [Required]
        public AttendeeStatusType Status { get; set; }
        
        public string GeneralCampaignId { get; set; }

        public GeneralCampaign GeneralCampaign { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}