using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Dtos.Company;
using TsheThauLoo.Dtos.File;

namespace TsheThauLoo.Dtos.Activity.GeneralCampaign
{
    public class GeneralCampaignDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "一般子活動識別碼")]
        public string Id { get; set; }
        
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

        [JsonPropertyName("limitNumberOfPeople")]
        [Display(Name = "人數限制")]
        public int LimitNumberOfPeople { get; set; }
        
        [JsonPropertyName("enableVerify")]
        [Display(Name = "審核")]
        public bool EnableVerify { get; set; }
        
        [JsonPropertyName("enableIdentityConfirmed")]
        [Display(Name = "實名審核")]
        public bool EnableIdentityConfirmed { get; set; }
        
        [JsonPropertyName("files")]
        [Display(Name = "一般活動附檔")]
        public IEnumerable<FileDto> Files { get; set; }

        [JsonPropertyName("company")]
        [Display(Name = "一般活動參與公司")]
        public CompanyDto Company { get; set; }
    }
}