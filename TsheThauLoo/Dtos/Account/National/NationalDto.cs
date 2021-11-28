using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Account.National
{
    public class NationalDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "使用者識別碼")]
        public string Id { get; set; }
        
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