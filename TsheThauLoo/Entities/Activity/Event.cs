using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般活動
    /// </summary>
    public class Event
    {
        /// <summary>
        /// 一般活動識別碼
        /// </summary>
        [Key]
        public string EventId { get; set; } = Guid.NewGuid().ToString();
        
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
        /// 聲明
        /// </summary>
        public string Declaration { get; set; }
        
        /// <summary>
        /// 地點
        /// </summary>
        [MaxLength(200)]
        public string Venue { get; set; }
        
        /// <summary>
        /// 報名開始時間
        /// </summary>
        public DateTime? RegistrationStartTime { get; set; }
        
        /// <summary>
        /// 報名結束時間
        /// </summary>
        public DateTime? RegistrationEndTime { get; set; }

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
        /// 人數限制
        /// </summary>
        [Required]
        public int LimitNumberOfPeople { get; set; } = 0;
        
        /// <summary>
        /// 啟用審核
        /// </summary>
        [Required]
        public bool EnableVerify { get; set; } = false;
        
        /// <summary>
        /// 啟用實名審核
        /// </summary>
        [Required]
        public bool EnableIdentityConfirmed { get; set; } = false;
        
        public ICollection<EventFile> EventFiles { get; set; }
        
        public ICollection<EventAttendee> EventAttendees { get; set; }

        public ICollection<EventParticipant> EventParticipants { get; set; }
    }
}