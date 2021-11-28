using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.MyCampaign
{
    public class MyCampaignDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "系列活動識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        [Display(Name = "名稱")]
        public string Title { get; set; }

        [JsonPropertyName("startTime")]
        [Display(Name = "開始時間")]
        public DateTime StartTime { get; set; }
        
        [JsonPropertyName("endTime")]
        [Display(Name = "結束時間")]
        public DateTime EndTime { get; set; }
        
        [JsonPropertyName("generalCampaigns")]
        [Display(Name = "一般子活動")]
        public IEnumerable<MyGeneralCampaignDto> GeneralCampaigns { get; set; }
    }
}