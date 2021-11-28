using System;
using System.ComponentModel.DataAnnotations;

namespace TsheThauLoo.Entities.Activity
{
    /// <summary>
    /// 一般活動附檔
    /// </summary>
    public class EventFile : Document
    {
        /// <summary>
        /// 一般活動附檔識別碼
        /// </summary>
        [Key]
        public string EventFileId { get; set; } = Guid.NewGuid().ToString();
        
        public string EventId { get; set; }

        public Event Event { get; set; }
    }
}