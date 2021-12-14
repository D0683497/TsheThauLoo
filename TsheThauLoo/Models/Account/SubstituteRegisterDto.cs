using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 職務代理人註冊
/// </summary>
public record SubstituteRegisterDto
{
    /// <summary>
    /// 姓名
    /// </summary>
    [JsonPropertyName("name")]
    [Display(Name = "姓名")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [JsonPropertyName("email")]
    [Display(Name = "電子郵件")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(256, ErrorMessage = "{0}不能超過 {1} 個字")]
    [EmailAddress(ErrorMessage = "{0}格式錯誤")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// 電話號碼
    /// </summary>
    [JsonPropertyName("phonenumber")]
    [Display(Name = "電話號碼")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// 地址
    /// </summary>
    [JsonPropertyName("address")]
    [Display(Name = "地址")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(256, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Address { get; set; } = null!;

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
}