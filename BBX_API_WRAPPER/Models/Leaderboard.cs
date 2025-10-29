using System.Text.Json.Serialization;

namespace BBX_API_WRAPPER.Models
{
    public class Leaderboard
    {
        [JsonPropertyName("player_rank")]
        public int PlayerRank { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }

        [JsonPropertyName("total_score")]
        public int TotalScore { get; set; }

        [JsonPropertyName("total_win_percentage")]
        public int TotalWinPercentage { get; set; }

        [JsonPropertyName("region")]
        public string Region { get; set; }
    }
}