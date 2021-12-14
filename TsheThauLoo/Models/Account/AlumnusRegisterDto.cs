using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 校友註冊
/// </summary>
public record AlumnusRegisterDto : RegisterDto
{
    // TODO: 畢業年度格式驗證
    /// <summary>
    /// 畢業年度
    /// </summary>
    [JsonPropertyName("dateofgraduation")]
    [Display(Name = "畢業年度")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(7, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string DateOfGraduation { get; set; } = null!;

    /// <summary>
    /// 學院
    /// </summary>
    [JsonPropertyName("college")]
    [Display(Name = "學院")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(20, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string College { get; set; } = null!;

    /// <summary>
    /// 系所
    /// </summary>
    [JsonPropertyName("department")]
    [Display(Name = "系所")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Department { get; set; } = null!;
}