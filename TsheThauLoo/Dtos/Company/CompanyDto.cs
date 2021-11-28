using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Company
{
    public class CompanyDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "公司識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("hasLogo")]
        [Display(Name = "公司標識")]
        public bool HasLogo { get; set; }
        
        [JsonPropertyName("companyConfirmed")]
        [Display(Name = "公司驗證")]
        public bool CompanyConfirmed { get; set; }
        
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

        [JsonPropertyName("industrialClassifications")]
        [Display(Name = "產業類別")]
        public IEnumerable<IndustrialClassificationDto> IndustrialClassifications { get; set; }
    }
}