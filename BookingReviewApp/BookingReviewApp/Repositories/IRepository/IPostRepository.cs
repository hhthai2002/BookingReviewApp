using BookingReviewApp.Entities;

namespace BookingReviewApp.Repositories
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<IEnumerable<Post>> GetPostsByUserIdAsync(Guid userId);
    }
}
