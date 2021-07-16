using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Dtos.Account.Profile.Manager;

namespace TsheThauLoo.Dtos.Manage
{
    public class ManagerManageDto : UserManageDto
    {
        [JsonPropertyName("managerConfirmed")]
        [Display(Name = "公司使用者驗證")]
        public bool ManagerConfirmed { get; set; }
        
        [JsonPropertyName("divisionName")]
        [Display(Name = "所屬部門/單位")]
        public string DivisionName { get; set; }
        
        [JsonPropertyName("jobTitle")]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }
        
        [JsonPropertyName("contactEmail")]
        [Display(Name = "聯絡用電子郵件")]
        public string ContactEmail { get; set; }
        
        [JsonPropertyName("contactPhone")]
        [Display(Name = "聯絡用電話號碼")]
        public string ContactPhone { get; set; }
        
        [JsonPropertyName("contactAddress")]
        [Display(Name = "聯絡用地址")]
        public string ContactAddress { get; set; }
        
        [JsonPropertyName("substitute")]
        [Display(Name = "職務代理人")]
        public SubstituteEditDto Substitute { get; set; }
    }
}