using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 企業使用者註冊
/// </summary>
public record EmployeeRegisterDto : RegisterDto
{
    /// <summary>
    /// 職稱
    /// </summary>
    [JsonPropertyName("title")]
    [Display(Name = "職稱")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(20, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 部門
    /// </summary>
    [JsonPropertyName("division")]
    [Display(Name = "部門")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(20, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Division { get; set; } = null!;

    /// <summary>
    /// 職務代理人
    /// </summary>
    [JsonPropertyName("substitute")]
    [Display(Name = "職務代理人")]
    [Required(ErrorMessage = "請填寫{0}")]
    public SubstituteRegisterDto Substitute { get; set; } = null!;
}