using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Dtos.Account.Profile.Administrator;

namespace TsheThauLoo.Dtos
{
    public class AboutDto
    {
        [JsonPropertyName("name")]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        
        [JsonPropertyName("jobTitle")]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }
        
        [JsonPropertyName("extension")]
        [Display(Name = "分機")]
        public string Extension { get; set; }
        
        [JsonPropertyName("contactEmail")]
        [Display(Name = "聯絡用電子郵件")]
        public string ContactEmail { get; set; }
        
        [JsonPropertyName("responsibilities")]
        [Display(Name = "負責業務")]
        public IEnumerable<ResponsibilityDto> Responsibilities { get; set; }
    }
}