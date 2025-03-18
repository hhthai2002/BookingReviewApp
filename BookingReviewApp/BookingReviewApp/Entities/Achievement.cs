using BookingReviewApp.Enums;
using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class Achievement
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public AchievementType Type { get; set; }

        public DateTime AwardedDate { get; set; } = DateTime.UtcNow;
    }
}
