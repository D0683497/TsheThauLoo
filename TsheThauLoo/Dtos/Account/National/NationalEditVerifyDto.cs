using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.National
{
    public class NationalEditVerifyDto
    {
        [JsonPropertyName("description")]
        [Display(Name = "說明")]
        public string Description { get; set; }
    }
}