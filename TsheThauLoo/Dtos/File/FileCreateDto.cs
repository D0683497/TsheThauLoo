using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;

namespace TsheThauLoo.Dtos.File
{
    public class FileCreateDto
    {
        [JsonPropertyName("type")]
        [Display(Name = "檔案類型")]
        public string Type { get; set; }
        
        [JsonPropertyName("name")]
        [Display(Name = "檔案名稱")]
        public string Name { get; set; }

        [JsonPropertyName("fileData")]
        [Display(Name = "檔案")]
        public IFormFile FileData { get; set; }
    }
}