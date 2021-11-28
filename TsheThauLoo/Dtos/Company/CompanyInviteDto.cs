using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Company
{
    public class CompanyInviteDto
    {
        [JsonPropertyName("companyId")]
        [Display(Name = "公司識別碼")]
        public string CompanyId { get; set; }
    }
}