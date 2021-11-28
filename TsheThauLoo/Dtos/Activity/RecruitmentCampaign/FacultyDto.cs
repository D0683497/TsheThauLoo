using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class FacultyDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "需求科系識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("description")]
        [Display(Name = "描述")]
        public string Description  { get; set; }
    }
}