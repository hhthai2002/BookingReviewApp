using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Entities
{
    public class BookingPost
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(255)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, MaxLength(200)]
        public string Location { get; set; }

        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public bool IsCompleted { get; set; } = false;

        public Guid BookerId { get; set; }
        public virtual User Booker { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }
    }
}
