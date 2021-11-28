using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Activity.MyEvent
{
    public class MyEventDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "一般活動識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        [Display(Name = "名稱")]
        public string Title { get; set; }

        [JsonPropertyName("venue")]
        [Display(Name = "地點")]
        public string Venue { get; set; }
        
        [JsonPropertyName("registrationStartTime")]
        [Display(Name = "報名開始時間")]
        public DateTime? RegistrationStartTime { get; set; }
        
        [JsonPropertyName("registrationEndTime")]
        [Display(Name = "報名結束時間")]
        public DateTime? RegistrationEndTime { get; set; }

        [JsonPropertyName("startTime")]
        [Display(Name = "開始時間")]
        public DateTime StartTime { get; set; }
        
        [JsonPropertyName("endTime")]
        [Display(Name = "結束時間")]
        public DateTime EndTime { get; set; }
        
        [JsonPropertyName("status")]
        [Display(Name = "參與狀態")]
        public AttendeeStatusType Status { get; set; }
    }
}