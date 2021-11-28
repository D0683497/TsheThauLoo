using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Password
{
    public class ForgetPasswordDto
    {
        [JsonPropertyName("email")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
    }
}