using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class TournamentData
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        [JsonPropertyName("player_db_id")]
        public int PlayerDBId { get; set; }

        [JsonPropertyName("wins")]
        public int Wins { get; set; }

        [JsonPropertyName("losses")]
        public int Losses { get; set; }

        [JsonPropertyName("rank")]
        public int Rank { get; set; }

        [JsonPropertyName("win_percentage")]
        public double WinPercentage { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }
    }
}