using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class Player
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("username")]
        public string Username { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }
    }
}