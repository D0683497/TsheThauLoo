using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.School;

/// <summary>
/// 更新學院
/// </summary>
public record UpdateCollegeDto
{
    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    [Display(Name = "名稱")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(20, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Name { get; set; } = null!;
}