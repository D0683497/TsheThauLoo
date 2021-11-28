using System;
using System.ComponentModel.DataAnnotations;
using TsheThauLoo.Entities.User;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般活動參與者
    /// </summary>
    public class EventAttendee
    {
        /// <summary>
        /// 一般活動參與者識別碼
        /// </summary>
        [Key]
        public string EventAttendeeId { get; set; } = Guid.NewGuid().ToString();
        
        /// <summary>
        /// 參與狀態
        /// </summary>
        [Required]
        public AttendeeStatusType Status { get; set; }
        
        public string EventId { get; set; }

        public Event Event { get; set; }
        
        public string ApplicationUserId { get; set; }
        
        public ApplicationUser ApplicationUser { get; set; }
    }
}