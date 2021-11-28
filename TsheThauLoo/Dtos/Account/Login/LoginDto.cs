using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Login
{
    public class LoginDto
    {
        [JsonPropertyName("userName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }
        
        [JsonPropertyName("password")]
        [Display(Name = "密碼")]
        public string Password { get; set; }
    }
}