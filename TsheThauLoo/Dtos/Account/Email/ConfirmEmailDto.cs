using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Email
{
    public class ConfirmEmailDto
    {
        [JsonPropertyName("userId")]
        [Display(Name = "使用者識別碼")]
        public string UserId { get; set; }
        
        [JsonPropertyName("token")]
        [Display(Name = "權杖")]
        public string Token { get; set; }
    }
}