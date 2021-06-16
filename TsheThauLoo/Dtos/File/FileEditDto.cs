using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.File
{
    public class FileEditDto
    {
        [JsonPropertyName("name")]
        [Display(Name = "檔案名稱")]
        public string Name { get; set; }
        
        [JsonPropertyName("extension")]
        [Display(Name = "副檔名")]
        public string Extension { get; set; }
    }
}