using BookingReviewApp.Data;
using BookingReviewApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingReviewApp.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Review>> GetReviewsByBookPostIdAsync(Guid bookingPostId)
        {
            return await _context.Reviews.Where(r => r.BookingPostId == bookingPostId).ToListAsync();
        }
    }
}
