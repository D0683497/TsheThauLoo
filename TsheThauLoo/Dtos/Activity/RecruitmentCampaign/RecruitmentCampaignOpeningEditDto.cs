using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Dtos.Activity.RecruitmentCampaign
{
    public class RecruitmentCampaignOpeningEditDto
    {
        [JsonPropertyName("divisionName")]
        [Display(Name = "職缺單位/部門")]
        public string DivisionName { get; set; }
        
        [JsonPropertyName("jobTitle")]
        [Display(Name = "職務名稱")]
        public string JobTitle { get; set; }
        
        [JsonPropertyName("jobDescription")]
        [Display(Name = "工作內容")]
        public string JobDescription { get; set; }
        
        [JsonPropertyName("workPlace")]
        [Display(Name = "工作地點")]
        public string WorkPlace { get; set; }
        
        [JsonPropertyName("salary")]
        [Display(Name = "薪資")]
        public string Salary { get; set; }
        
        [JsonPropertyName("requiredNumber")]
        [Display(Name = "需求人數")]
        public int RequiredNumber { get; set; }
        
        [JsonPropertyName("education")]
        [Display(Name = "學歷")]
        public EducationType Education { get; set; }
        
        [JsonPropertyName("workExperience")]
        [Display(Name = "相關工作經驗")]
        public string WorkExperience { get; set; }
        
        [JsonPropertyName("language")]
        [Display(Name = "語言能力")]
        public string Language { get; set; }
        
        [JsonPropertyName("nationality")]
        [Display(Name = "聘用人員國籍")]
        public string Nationality { get; set; }
        
        [JsonPropertyName("isAccessibility")]
        [Display(Name = "身心障礙者應徵")]
        public bool IsAccessibility { get; set; }
    }
}