using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Company
{
    public class IndustrialClassificationDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "產業類別識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("description")]
        [Display(Name = "說明")]
        public string Description { get; set; }
    }
}