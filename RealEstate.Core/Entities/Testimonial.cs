
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RealEstate.Domain.Entities
{
    public class Testimonial
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }

        [Required]
        [MaxLength(100)]
        public string Title { get; set; }

        [Required]
        [MaxLength(1000)]
        public string Message { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public DateTime SubmittedOn { get; set; } = DateTime.UtcNow;
    }
}
