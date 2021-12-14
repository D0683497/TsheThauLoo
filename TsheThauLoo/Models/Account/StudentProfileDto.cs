using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

public record StudentProfileDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 學號
    /// </summary>
    [JsonPropertyName("networkid")]
    public string NetworkId { get; set; } = null!;

    /// <summary>
    /// 學院
    /// </summary>
    [JsonPropertyName("college")]
    public string College { get; set; } = null!;

    /// <summary>
    /// 系所
    /// </summary>
    [JsonPropertyName("department")]
    public string Department { get; set; } = null!;
}