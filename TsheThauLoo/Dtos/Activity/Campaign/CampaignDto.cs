using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Dtos.File;

namespace TsheThauLoo.Dtos.Activity.Campaign
{
    public class CampaignDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "系列活動識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("title")]
        [Display(Name = "名稱")]
        public string Title { get; set; }
        
        [JsonPropertyName("content")]
        [Display(Name = "內容")]
        public string Content { get; set; }

        [JsonPropertyName("startTime")]
        [Display(Name = "開始時間")]
        public DateTime StartTime { get; set; }
        
        [JsonPropertyName("endTime")]
        [Display(Name = "結束時間")]
        public DateTime EndTime { get; set; }
        
        [JsonPropertyName("files")]
        [Display(Name = "系列活動附檔")]
        public IEnumerable<FileDto> Files { get; set; }
    }
}