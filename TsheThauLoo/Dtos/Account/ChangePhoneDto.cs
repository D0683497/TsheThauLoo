using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account
{
    public class ChangePhoneDto
    {
        [JsonPropertyName("newPhoneNumber")]
        [Display(Name = "新的手機號碼")]
        public string NewPhoneNumber { get; set; }
    }
}