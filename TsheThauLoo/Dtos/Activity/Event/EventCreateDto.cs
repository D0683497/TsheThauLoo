using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.Event
{
    public class EventCreateDto
    {
        [JsonPropertyName("title")]
        [Display(Name = "名稱")]
        public string Title { get; set; }
        
        [JsonPropertyName("content")]
        [Display(Name = "內容")]
        public string Content { get; set; }
        
        [JsonPropertyName("declaration")]
        [Display(Name = "聲明")]
        public string Declaration { get; set; }
        
        [JsonPropertyName("venue")]
        [Display(Name = "地點")]
        public string Venue { get; set; }
        
        [JsonPropertyName("registrationStartDate")]
        [Display(Name = "報名開始日期")]
        public DateTime? RegistrationStartDate { get; set; }
        
        [JsonPropertyName("registrationStartTime")]
        [Display(Name = "報名開始時間")]
        public DateTime? RegistrationStartTime { get; set; }
        
        [JsonPropertyName("registrationEndDate")]
        [Display(Name = "報名結束日期")]
        public DateTime? RegistrationEndDate { get; set; }
        
        [JsonPropertyName("registrationEndTime")]
        [Display(Name = "報名結束時間")]
        public DateTime? RegistrationEndTime { get; set; }

        [JsonPropertyName("startDate")]
        [Display(Name = "開始日期")]
        public DateTime StartDate { get; set; }
        
        [JsonPropertyName("startTime")]
        [Display(Name = "開始時間")]
        public DateTime StartTime { get; set; }
        
        [JsonPropertyName("endDate")]
        [Display(Name = "結束日期")]
        public DateTime EndDate { get; set; }
        
        [JsonPropertyName("endTime")]
        [Display(Name = "結束時間")]
        public DateTime EndTime { get; set; }

        [JsonPropertyName("limitNumberOfPeople")]
        [Display(Name = "人數限制")]
        public int LimitNumberOfPeople { get; set; }
        
        [JsonPropertyName("enableVerify")]
        [Display(Name = "審核")]
        public bool EnableVerify { get; set; }
        
        [JsonPropertyName("enableIdentityConfirmed")]
        [Display(Name = "實名審核")]
        public bool EnableIdentityConfirmed { get; set; }
    }
}