using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Models.School;

/// <summary>
/// 更新系所
/// </summary>
public class UpdateDepartmentDto
{
    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    [Display(Name = "名稱")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 學位
    /// </summary>
    [JsonPropertyName("degree")]
    [Display(Name = "學位")]
    [Required(ErrorMessage = "請填寫{0}")]
    [EnumDataType(typeof(DegreeType), ErrorMessage = "請選擇正確的{0}")]
    public DegreeType? Degree { get; set; } = null;
}