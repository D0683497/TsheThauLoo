using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TsheThauLoo.Dtos.Account.Login
{
    public class NIDLoginDto
    {
        [BindProperty(Name = "status")]
        [JsonPropertyName("status")]
        public int Status { get; set; }

        [BindProperty(Name = "message")]
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [BindProperty(Name = "user_code")]
        [JsonPropertyName("user_code")]
        public string UserCode { get; set; }
    }
}