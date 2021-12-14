using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 註冊
/// </summary>
public record RegisterDto
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
    public string Password { get; set; }= null!;

    /// <summary>
    /// 確認密碼
    /// </summary>
    [JsonPropertyName("passwordconfirm")]
    [Display(Name = "確認密碼")]
    [Required(ErrorMessage = "請填寫{0}")]
    [Compare(nameof(Password), ErrorMessage = "{0}錯誤")]
    public string PasswordConfirm { get; set; }= null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [JsonPropertyName("email")]
    [Display(Name = "電子郵件")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(256, ErrorMessage = "{0}不能超過 {1} 個字")]
    [EmailAddress(ErrorMessage = "{0}格式錯誤")]
    public string Email { get; set; }= null!;

    // TODO: 電話號碼格式驗證
    /// <summary>
    /// 電話號碼
    /// </summary>
    [JsonPropertyName("phonenumber")]
    [Display(Name = "電話號碼")]
    [StringLength(30, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string? PhoneNumber { get; set; } = null;

    // TODO: 身分證字號格式驗證
    /// <summary>
    /// 身分證字號
    /// </summary>
    [JsonPropertyName("nationalid")]
    [Display(Name = "身分證字號")]
    [StringLength(15, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string? NationalId { get; set; } = null;

    /// <summary>
    /// 姓名
    /// </summary>
    [JsonPropertyName("name")]
    [Display(Name = "姓名")]
    [Required(ErrorMessage = "請填寫{0}")]
    [StringLength(50, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string Name { get; set; }= null!;

    /// <summary>
    /// 性別
    /// </summary>
    [JsonPropertyName("gender")]
    [Display(Name = "性別")]
    [EnumDataType(typeof(GenderType), ErrorMessage = "請選擇正確的{0}")]
    public GenderType? Gender { get; set; } = null;

    // TODO: 生日格式驗證
    /// <summary>
    /// 生日
    /// </summary>
    [JsonPropertyName("dateofbirth")]
    [Display(Name = "生日")]
    [StringLength(10, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string? DateOfBirth { get; set; } = null;

    /// <summary>
    /// 地址
    /// </summary>
    [JsonPropertyName("address")]
    [Display(Name = "地址")]
    [StringLength(256, ErrorMessage = "{0}不能超過 {1} 個字")]
    public string? Address { get; set; } = null;
}