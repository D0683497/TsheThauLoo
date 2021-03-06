using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Administrator
{
    public class AdministratorInfoDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "使用者識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("administratorConfirmed")]
        [Display(Name = "管理員驗證")]
        public bool AdministratorConfirmed { get; set; }
        
        [JsonPropertyName("showAbout")]
        [Display(Name = "顯示於關於頁面")]
        public bool ShowAbout { get; set; }
        
        [JsonPropertyName("networkId")]
        [Display(Name = "證號")]
        public string NetworkId { get; set; }
        
        [JsonPropertyName("dept")]
        [Display(Name = "部門")]
        public string Dept { get; set; }

        [JsonPropertyName("unit")]
        [Display(Name = "單位")]
        public string Unit { get; set; }

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