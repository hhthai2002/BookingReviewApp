namespace BookingReviewApp.Dtos
{
    public class PostShareDto
    {
        public Guid Id { get; set; }
        public Guid PostId { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public DateTime SharedAt { get; set; }
    }
}
