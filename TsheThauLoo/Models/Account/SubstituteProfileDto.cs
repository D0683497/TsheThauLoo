using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

public record SubstituteProfileDto
{
    /// <summary>
    /// 姓名
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 電子郵件
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; } = null!;

    /// <summary>
    /// 電話號碼
    /// </summary>
    [JsonPropertyName("phonenumber")]
    public string PhoneNumber { get; set; } = null!;

    /// <summary>
    /// 地址
    /// </summary>
    [JsonPropertyName("address")]
    public string Address { get; set; } = null!;

    /// <summary>
    /// 職稱
    /// </summary>
    [JsonPropertyName("title")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 部門
    /// </summary>
    [JsonPropertyName("division")]
    public string Division { get; set; } = null!;
}