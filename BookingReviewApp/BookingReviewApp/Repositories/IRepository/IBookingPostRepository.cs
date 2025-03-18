using BookingReviewApp.Entities;

namespace BookingReviewApp.Repositories
{
    public interface IBookingPostRepository : IRepository<BookingPost>
    {
        Task<IEnumerable<BookingPost>> GetByUserIdAsync(Guid userId);
    }
}
