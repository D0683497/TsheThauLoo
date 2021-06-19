using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Email
{
    public class ChangeEmailDto
    {
        [JsonPropertyName("newEmail")]
        [Display(Name = "新的電子郵件")]
        public string NewEmail { get; set; }
    }
}