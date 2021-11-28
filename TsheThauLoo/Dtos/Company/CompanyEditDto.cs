using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Company
{
    public class CompanyEditDto
    {
        [JsonPropertyName("registrationNumber")]
        [Display(Name = "統一編號")]
        public string RegistrationNumber { get; set; }
        
        [JsonPropertyName("name")]
        [Display(Name = "名稱")]
        public string Name { get; set; }
        
        [JsonPropertyName("introduction")]
        [Display(Name = "簡介")]
        public string Introduction { get; set; }
        
        [JsonPropertyName("website")]
        [Display(Name = "網站")]
        public string Website { get; set; }
    }
}