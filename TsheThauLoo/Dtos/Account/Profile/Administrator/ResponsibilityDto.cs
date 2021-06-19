using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Administrator
{
    public class ResponsibilityDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("description")]
        [Display(Name = "描述")]
        public string Description  { get; set; }
    }
}