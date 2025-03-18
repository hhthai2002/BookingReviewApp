using BookingReviewApp.Entities;

namespace BookingReviewApp.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetUserByEmailAsync(string email);
        Task<User> GetUserByResetTokenAsync(string token);
        Task<User> GetUserByVerificationTokenAsync(string token);

    }
}
