using System;
using System.ComponentModel.DataAnnotations;

namespace neuron_gym.Models
{
    public class Score
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string PlayerName { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string Game { get; set; } = string.Empty;

        [Range(0, int.MaxValue)]
        public int Value { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
