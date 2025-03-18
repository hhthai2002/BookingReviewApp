namespace BookingReviewApp.Dtos
{
    public class AchievementDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; } // convert enum to string
        public DateTime AwardedDate { get; set; }
    }
}
