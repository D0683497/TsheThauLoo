using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account
{
    public class ChangeUserNameDto
    {
        [JsonPropertyName("newUserName")]
        [Display(Name = "新的使用者名稱")]
        public string NewUserName { get; set; }
    }
}