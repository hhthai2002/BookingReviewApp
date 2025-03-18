namespace BookingReviewApp.Dtos
{
    public class ReportDto
    {
        public Guid Id { get; set; }
        public Guid BookingPostId { get; set; }
        public string BookTitle { get; set; }
        public string Reason { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
