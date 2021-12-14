using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 
/// </summary>
public record EmployeeProfileDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

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

    /// <summary>
    /// 職務代理人
    /// </summary>
    [JsonPropertyName("substitute")]
    public SubstituteProfileDto Substitute { get; set; } = null!;
}