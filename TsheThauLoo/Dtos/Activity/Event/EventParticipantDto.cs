using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace TsheThauLoo.Dtos.Activity.Event
{
    public class EventParticipantDto
    {
        [JsonPropertyName("name")]
        [Display(Name = "姓名")]
        public string Name { get; set; }
        
        [JsonPropertyName("contactPhone")]
        [Display(Name = "聯絡用電話號碼")]
        public string ContactPhone { get; set; }
        
        [JsonPropertyName("remark")]
        [Display(Name = "備註")]
        public string Remark { get; set; }
    }
}