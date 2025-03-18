using BookingReviewApp.Entities;

namespace BookingReviewApp.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByBookPostIdAsync(Guid bookPostId);
    }
}
