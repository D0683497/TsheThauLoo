using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Manage
{
    public class UserManageDto
    {
        [JsonPropertyName("userName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }

        [JsonPropertyName("email")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
        
        [JsonPropertyName("emailConfirmed")]
        [Display(Name = "電子郵件驗證")]
        public bool EmailConfirmed { get; set; }
        
        [JsonPropertyName("phoneNumber")]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }
        
        [JsonPropertyName("phoneNumberConfirmed")]
        [Display(Name = "手機號碼驗證")]
        public bool PhoneNumberConfirmed { get; set; }
        
        [JsonPropertyName("lockoutEnd")]
        [Display(Name = "鎖定結束時間")]
        public DateTimeOffset? LockoutEnd { get; set; }
        
        [JsonPropertyName("lockoutEnabled")]
        [Display(Name = "可以鎖定使用者")]
        public bool LockoutEnabled { get; set; }
        
        [JsonPropertyName("accessFailedCount")]
        [Display(Name = "登入失敗次數")]
        public int AccessFailedCount { get; set; }
        
        [JsonPropertyName("isEnable")]
        [Display(Name = "啟用帳戶")]
        public bool IsEnable { get; set; }

        [JsonPropertyName("identityConfirmed")]
        [Display(Name = "實名驗證")]
        public bool IdentityConfirmed { get; set; }

        [JsonPropertyName("nationalId")]
        [Display(Name = "身份證字號")]
        public string NationalId { get; set; }
        
        [JsonPropertyName("name")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [JsonPropertyName("gender")]
        [Display(Name = "性別")]
        public GenderType? Gender { get; set; }
        
        [JsonPropertyName("dateOfBirth")]
        [Display(Name = "生日")]
        public DateTime? DateOfBirth { get; set; }
        
        [JsonPropertyName("currentAddress")]
        [Display(Name = "通訊地址")]
        public string CurrentAddress { get; set; }
    }
}