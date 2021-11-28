using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Alumnus
{
    public class AlumnusEditVerifyDto
    {
        [JsonPropertyName("description")]
        [Display(Name = "說明")]
        public string Description { get; set; }
    }
}