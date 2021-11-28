using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class RecruitmentCampaignEditDto
    {
        [JsonPropertyName("title")]
        [Display(Name = "名稱")]
        public string Title { get; set; }
        
        [JsonPropertyName("content")]
        [Display(Name = "內容")]
        public string Content { get; set; }
        
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
        
        [JsonPropertyName("enableReview")]
        [Display(Name = "審查")]
        public bool EnableReview { get; set; }
    }
}