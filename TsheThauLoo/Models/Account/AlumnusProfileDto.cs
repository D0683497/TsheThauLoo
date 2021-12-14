using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.Account;

/// <summary>
/// 
/// </summary>
public record AlumnusProfileDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 畢業年度
    /// </summary>
    [JsonPropertyName("dateofgraduation")]
    public string DateOfGraduation { get; set; } = null!;

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