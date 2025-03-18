using System.ComponentModel.DataAnnotations;

namespace BookingReviewApp.Dtos
{
    public class BookingPostDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        public Guid BookerId { get; set; }
    }
}
