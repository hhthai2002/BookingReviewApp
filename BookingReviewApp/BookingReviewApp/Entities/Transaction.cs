using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class Transaction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required, MaxLength(20)]
        public string TransactionType { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
    }
}
