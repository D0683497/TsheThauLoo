using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Password
{
    public class ChangePasswordDto
    {
        [JsonPropertyName("currentPassword")]
        [Display(Name = "目前密碼")]
        public string CurrentPassword { get; set; }
        
        [JsonPropertyName("newPassword")]
        [Display(Name = "新密碼")]
        public string NewPassword { get; set; }
        
        [JsonPropertyName("confirmNewPassword")]
        [Display(Name = "確認新密碼")]
        public string ConfirmNewPassword { get; set; }
    }
}