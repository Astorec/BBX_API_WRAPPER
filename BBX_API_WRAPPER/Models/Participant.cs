using System.ComponentModel.DataAnnotations;

namespace BBX_API_WRAPPER.Models
{
    public class Participant
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int PlayerDBId { get; set; }

        [Required]
        public int TournamentId { get; set; }

        [Required]
        public int PlayerId { get; set; }

        public int GroupId { get; set; }
    }
}