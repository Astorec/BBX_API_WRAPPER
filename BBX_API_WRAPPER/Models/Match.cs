using System.ComponentModel.DataAnnotations;

namespace BBX_API_WRAPPER.Models
{
    public class Match
    {
        [Required]
        public int Id { get; set; }

        public int Player1Id { get; set; }

        public int Player2Id { get; set; }

        public int WinnerId { get; set; }
        public int LoserId { get; set; }

        [Required]
        public int TournamentId { get; set; }

        [Required]
        public int IsFinals { get; set; }

        [Required]
        public int MatchId { get; set; }

        [Required]
        public int Round { get; set; }

        public int? Player1Score { get; set; }
        public int? Player2Score { get; set; }
    }
}