using System.Text.Json.Serialization;
using TsheThauLoo.Enums;

namespace TsheThauLoo.Models.School
{
    /// <summary>
    /// 系所
    /// </summary>
    public class DepartmentDto
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

        /// <summary>
        /// 學位
        /// </summary>
        [JsonPropertyName("degree")]
        public DegreeType Degree { get; set; }
    }
}
