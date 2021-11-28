using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Dtos.File;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class ResumeDeliveryListDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "徵才子活動履歷識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("type")]
        [Display(Name = "履歷審查狀態")]
        public ResumeReviewType Type { get; set; }
        
        [JsonPropertyName("isInterview")]
        [Display(Name = "面試機會")]
        public bool IsInterview { get; set; }
        
        [JsonPropertyName("isHire")]
        [Display(Name = "錄取")]
        public bool IsHire { get; set; }

        [JsonPropertyName("resume")]
        [Display(Name = "履歷")]
        public FileDto Resume { get; set; }
    }
}