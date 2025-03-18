using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class PostLike
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("Post")]
        public Guid PostId { get; set; }
        public Post Post { get; set; }

        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
