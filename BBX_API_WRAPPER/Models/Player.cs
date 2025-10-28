using System.ComponentModel.DataAnnotations;

namespace BBX_API_WRAPPER.Models
{
    public class Player
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Username { get; set; }

        [Required]
        public int Region { get; set; }
    }
}