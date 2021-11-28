using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Register
{
    public class EmployeeRegisterDto : UserRegisterDto
    {
        [JsonPropertyName("networkId")]
        [Display(Name = "證號")]
        public string NetworkId { get; set; }
        
        [JsonPropertyName("dept")]
        [Display(Name = "部門(學院)")]
        public string Dept { get; set; }
        
        [JsonPropertyName("unit")]
        [Display(Name = "單位(系所)")]
        public string Unit { get; set; }
    }
}