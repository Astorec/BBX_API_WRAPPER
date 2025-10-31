using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class Match
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("player1_id")]
        public int? Player1Id { get; set; }

        [JsonPropertyName("player2_id")]
        public int? Player2Id { get; set; }

        [JsonPropertyName("winner_id")]
        public int? WinnerId { get; set; }

        [JsonPropertyName("loser_id")]
        public int? LoserId { get; set; }

        [JsonPropertyName("tournament_id")]
        public int TournamentId { get; set; }

        [JsonPropertyName("is_finals")]
        public int IsFinals { get; set; }

        [JsonPropertyName("match_id")]
        public int MatchId { get; set; }

        // This is only null as some data is missing from older tournaments
        [JsonPropertyName("round")]
        public int? Round { get; set; }

        [JsonPropertyName("player1_score")]
        public int? Player1Score { get; set; }

        [JsonPropertyName("player2_score")]
        public int? Player2Score { get; set; }
    }
}