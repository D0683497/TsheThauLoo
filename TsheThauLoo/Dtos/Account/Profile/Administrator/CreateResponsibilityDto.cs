using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Account.Profile.Administrator
{
    public class CreateResponsibilityDto
    {
        [JsonPropertyName("description")]
        [Display(Name = "描述")]
        public string Description  { get; set; }
    }
}