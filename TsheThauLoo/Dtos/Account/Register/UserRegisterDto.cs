using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Account.Register
{
    public class UserRegisterDto
    {
        [JsonPropertyName("userName")]
        [Display(Name = "使用者名稱")]
        public string UserName { get; set; }

        [JsonPropertyName("password")]
        [Display(Name = "密碼")]
        public string Password { get; set; }
        
        [JsonPropertyName("passwordConfirm")]
        [Display(Name = "確認密碼")]
        public string PasswordConfirm { get; set; }
        
        [JsonPropertyName("email")]
        [Display(Name = "電子郵件")]
        public string Email { get; set; }
        
        [JsonPropertyName("phoneNumber")]
        [Display(Name = "手機號碼")]
        public string PhoneNumber { get; set; }
        
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