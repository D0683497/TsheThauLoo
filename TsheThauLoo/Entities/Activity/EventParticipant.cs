using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般活動現場報名者
    /// </summary>
    public class EventParticipant
    {
        /// <summary>
        /// 一般活動現場報名者識別碼
        /// </summary>
        [Key]
        public string EventParticipantId { get; set; } = Guid.NewGuid().ToString();
        
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
        
        public string EventId { get; set; }

        public Event Event { get; set; }
    }
}