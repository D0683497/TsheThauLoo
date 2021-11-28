using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Manage
{
    public class ExaminerDto : UserDto
    {
        [JsonPropertyName("examinerConfirmed")]
        [Display(Name = "審查員驗證")]
        public bool ExaminerConfirmed { get; set; }
        
        [JsonPropertyName("divisionName")]
        [Display(Name = "工作單位")]
        public string DivisionName { get; set; }
        
        [JsonPropertyName("jobTitle")]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }
    }
}