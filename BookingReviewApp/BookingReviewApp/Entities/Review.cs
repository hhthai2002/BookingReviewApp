using BookingReviewApp.Entities;
using System.ComponentModel.DataAnnotations;

public class Review
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid BookingPostId { get; set; }
    public virtual BookingPost BookingPost { get; set; }

    public Guid ReviewerId { get; set; }
    public virtual User Reviewer { get; set; }

    public bool IsCompleted { get; set; } = false;

    [Range(0, double.MaxValue)]
    public decimal AmountPaid { get; set; }

    [Range(1, 5)]
    public int Rating { get; set; } // rate from 1 to 5
}
