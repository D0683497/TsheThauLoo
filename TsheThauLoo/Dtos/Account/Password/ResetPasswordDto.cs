using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Password
{
    public class ResetPasswordDto
    {
        [JsonPropertyName("userId")]
        [Display(Name = "使用者識別碼")]
        public string UserId { get; set; }
        
        [JsonPropertyName("token")]
        [Display(Name = "權杖")]
        public string Token { get; set; }
        
        [JsonPropertyName("password")]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        
        [JsonPropertyName("passwordConfirm")]
        [Display(Name = "確認密碼")]
        public string PasswordConfirm { get; set; }
    }
}