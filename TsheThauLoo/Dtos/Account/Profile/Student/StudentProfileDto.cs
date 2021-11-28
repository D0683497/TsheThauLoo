using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Student
{
    public class StudentProfileDto : UserProfileDto
    {
        [JsonPropertyName("studentConfirmed")]
        [Display(Name = "在校生驗證")]
        public bool StudentConfirmed { get; set; }
        
        [JsonPropertyName("networkId")]
        [Display(Name = "學號")]
        public string NetworkId { get; set; }
        
        [JsonPropertyName("college")]
        [Display(Name = "學院")]
        public string College { get; set; }

        [JsonPropertyName("department")]
        [Display(Name = "系所")]
        public string Department { get; set; }
        
        [JsonPropertyName("class")]
        [Display(Name = "班級")]
        public string Class { get; set; }
    }
}