using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 登入
/// </summary>
public record LoginDto
{
    /// <summary>
    /// 帳號
    /// </summary>
    [JsonPropertyName("username")]
    [Display(Name = "帳號")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(64, MinimumLength = 3, ErrorMessage = "{0}長度需介於 {2} 到 {1} 之間")]
    [RegularExpression("^[abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789]+$", ErrorMessage = "{0} 只能是大小寫字母或數字")]
    public string UserName { get; set; } = null!;

    /// <summary>
    /// 密碼
    /// </summary>
    [JsonPropertyName("password")]
    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(64, MinimumLength = 8, ErrorMessage = "{0}長度需介於 {2} 到 {1} 之間")]
    public string Password { get; set; } = null!;
}