using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Alumnus
{
    public class AlumnusInfoDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "使用者識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("alumnusConfirmed")]
        [Display(Name = "校友驗證")]
        public bool AlumnusConfirmed { get; set; }
        
        [JsonPropertyName("dateOfGraduation")]
        [Display(Name = "畢業年度")]
        public string DateOfGraduation { get; set; }

        [JsonPropertyName("college")]
        [Display(Name = "畢業學院")]
        public string College { get; set; }

        [JsonPropertyName("department")]
        [Display(Name = "畢業系所")]
        public string Department { get; set; }
        
        [JsonPropertyName("class")]
        [Display(Name = "畢業班級")]
        public string Class { get; set; }
    }
}