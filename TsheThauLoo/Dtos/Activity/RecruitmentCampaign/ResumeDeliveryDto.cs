using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class ResumeDeliveryDto
    {
        [JsonPropertyName("resumeId")]
        [Display(Name = "履歷識別碼")]
        public string ResumeId { get; set; }
    }
}