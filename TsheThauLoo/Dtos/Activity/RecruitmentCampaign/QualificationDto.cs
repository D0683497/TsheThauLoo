using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class QualificationDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "資格條件識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("description")]
        [Display(Name = "描述")]
        public string Description  { get; set; }
    }
}