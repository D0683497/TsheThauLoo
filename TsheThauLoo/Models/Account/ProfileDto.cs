using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 
/// </summary>
public record ProfileDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
    
    /// <summary>
    /// 帳號
    /// </summary>
    [JsonPropertyName("username")]
    public string UserName { get; set; } = null!;
    
    /// <summary>
    /// 電子郵件
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }= null!;
    
    /// <summary>
    /// 電話號碼
    /// </summary>
    [JsonPropertyName("phonenumber")]
    public string? PhoneNumber { get; set; } = null;

    /// <summary>
    /// 身分證字號
    /// </summary>
    [JsonPropertyName("nationalid")]
    public string? NationalId { get; set; } = null;

    /// <summary>
    /// 姓名
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }= null!;

    /// <summary>
    /// 性別
    /// </summary>
    [JsonPropertyName("gender")]
    public GenderType? Gender { get; set; } = null;

    /// <summary>
    /// 生日
    /// </summary>
    [JsonPropertyName("dateofbirth")]
    public string? DateOfBirth { get; set; } = null;

    /// <summary>
    /// 地址
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; set; } = null;
}