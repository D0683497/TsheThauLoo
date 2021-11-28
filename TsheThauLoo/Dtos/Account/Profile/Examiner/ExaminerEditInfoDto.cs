using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Examiner
{
    public class ExaminerEditInfoDto
    {
        [JsonPropertyName("divisionName")]
        [Display(Name = "工作單位")]
        public string DivisionName { get; set; }
        
        [JsonPropertyName("jobTitle")]
        [Display(Name = "職稱")]
        public string JobTitle { get; set; }
    }
}