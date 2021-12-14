using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

public record StaffProfileDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 證號
    /// </summary>
    [JsonPropertyName("networkid")]
    public string NetworkId { get; set; } = null!;

    /// <summary>
    /// 部門(學院)
    /// </summary>
    [JsonPropertyName("dept")]
    public string Dept { get; set; } = null!;

    /// <summary>
    /// 單位(系所)
    /// </summary>
    [JsonPropertyName("unit")]
    public string? Unit { get; set; } = null;
}