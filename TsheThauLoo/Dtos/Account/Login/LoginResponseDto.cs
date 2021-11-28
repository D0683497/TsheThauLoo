using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Login
{
    public class LoginResponseDto
    {
        [JsonPropertyName("accessToken")]
        [Display(Name = "AccessToken")]
        public string AccessToken { get; set; }
    }
}