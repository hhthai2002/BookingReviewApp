using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class Report
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid BookingPostId { get; set; }
        public virtual BookingPost BookingPost { get; set; }

        [Required, MaxLength(500)]
        public string Reason { get; set; }

        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
    }
}
