namespace BBX_API_WRAPPER.Models
{
    public class Leaderboard
    {
        public int player_rank { get; set; }
        public string display_name { get; set; }
        public int total_score { get; set; }
        public int total_win_percentage { get; set; }
        public string region { get; set; }
    }
}