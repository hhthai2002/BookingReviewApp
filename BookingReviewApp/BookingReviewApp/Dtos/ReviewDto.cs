namespace BookingReviewApp.Dtos
{
    public class ReviewDto
    {
        public Guid Id { get; set; }
        public Guid BookingPostId { get; set; }
        public string BookTitle { get; set; }
        public Guid ReviewerId { get; set; }
        public string ReviewerName { get; set; }
        public bool IsCompleted { get; set; }
        public decimal AmountPaid { get; set; }
        public int Rating { get; set; }
    }
}
