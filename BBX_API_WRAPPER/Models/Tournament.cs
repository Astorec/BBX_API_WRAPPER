using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class Tournament
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("participants")]
        public int? Participants { get; set; }

        [JsonPropertyName("is_side_event")]
        public int IsSideEvent { get; set; }

        [JsonPropertyName("region")]
        public int Region { get; set; }

        [JsonPropertyName("attendance_id")]
        public int AttendanceId { get; set; }

        [JsonPropertyName("finalized")]
        public int Finalized { get; set; }

        [JsonPropertyName("state")]
        public string State { get; set; }

        [JsonPropertyName("is_store_championship")]
        public int IsStoreChampionship { get; set; }
    }
}