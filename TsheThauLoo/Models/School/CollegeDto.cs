using System.Text.Json.Serialization;

namespace TsheThauLoo.Models.School;

/// <summary>
/// 學院
/// </summary>
public class CollegeDto
{
    /// <summary>
    /// 識別碼
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;

    /// <summary>
    /// 名稱
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;
}