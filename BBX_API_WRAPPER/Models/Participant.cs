using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class Participant
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("player_db_id")]
        public int PlayerDBId { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        [JsonPropertyName("player_id")]
        public int PlayerId { get; set; }

        [JsonPropertyName("group_id")]
        public int GroupId { get; set; }
    }
}