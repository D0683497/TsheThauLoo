using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Company
{
    public class IndustrialClassificationCreateDto
    {
        [JsonPropertyName("description")]
        [Display(Name = "說明")]
        public string Description { get; set; }
    }
}