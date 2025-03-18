namespace BookingReviewApp.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public decimal WalletBalance { get; set; }
        public string Role { get; set; } // convert Role enum to string
    }
}
