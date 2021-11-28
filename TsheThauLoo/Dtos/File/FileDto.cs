using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.File
{
    public class FileDto
    {
        [JsonPropertyName("id")]
        [Display(Name = "檔案識別碼")]
        public string Id { get; set; }
        
        [JsonPropertyName("type")]
        [Display(Name = "檔案類型")]
        public string Type { get; set; }
        
        [JsonPropertyName("name")]
        [Display(Name = "檔案名稱")]
        public string Name { get; set; }

        [JsonPropertyName("extension")]
        [Display(Name = "副檔名")]
        public string Extension { get; set; }
    }
}