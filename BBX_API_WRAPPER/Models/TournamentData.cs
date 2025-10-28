using System.ComponentModel.DataAnnotations;

namespace BBX_API_WRAPPER.Models
{
    public class TournamentData
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int TournamentId { get; set; }
        [Required]
        public int PlayerDBId { get; set; }

        public int Wins { get; set; }
        public int Losses { get; set; }
        public int Rank { get; set; }
        public double WinPercentage { get; set; }
        public int Score { get; set; }
    }
}