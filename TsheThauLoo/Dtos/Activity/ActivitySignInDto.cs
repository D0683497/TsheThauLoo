using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity
{
    public class ActivitySignInDto
    {
        [JsonPropertyName("userId")]
        [Display(Name = "使用者識別碼")]
        public string UserId { get; set; }
    }
}