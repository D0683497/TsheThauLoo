using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class FacultyEditDto
    {
        [JsonPropertyName("description")]
        [Display(Name = "描述")]
        public string Description  { get; set; }
    }
}