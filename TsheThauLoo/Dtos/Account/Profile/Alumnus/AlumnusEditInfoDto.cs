using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Alumnus
{
    public class AlumnusEditInfoDto
    {
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