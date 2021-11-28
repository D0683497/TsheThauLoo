using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

namespace TsheThauLoo.Dtos.Account.Login
{
    public class NIDUserInfoDto
    {
        [BindProperty(Name = "id")]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [BindProperty(Name = "name")]
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [BindProperty(Name = "type")]
        [JsonPropertyName("type")]
        public string Type { get; set; }

        [BindProperty(Name = "classname")]
        [JsonPropertyName("classname")]
        public string ClassName { get; set; }

        [BindProperty(Name = "unit_id")]
        [JsonPropertyName("unit_id")]
        public string UnitId { get; set; }

        [BindProperty(Name = "unit_name")]
        [JsonPropertyName("unit_name")]
        public string UnitName { get; set; }

        [BindProperty(Name = "dept_id")]
        [JsonPropertyName("dept_id")]
        public string DeptId { get; set; }

        [BindProperty(Name = "dept_name")]
        [JsonPropertyName("dept_name")]
        public string DeptName { get; set; }

        [BindProperty(Name = "Email")]
        [JsonPropertyName("Email")]
        public string Email { get; set; }
    }
}