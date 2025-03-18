using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class PostShare
    {
        [Key]
        public Guid Id { get; set; }

        public DateTime SharedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}
