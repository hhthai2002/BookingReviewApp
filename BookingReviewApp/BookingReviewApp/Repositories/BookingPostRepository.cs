using BookingReviewApp.Data;
using BookingReviewApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace BookingReviewApp.Repositories
{
    public class BookingPostRepository : Repository<BookingPost>, IBookingPostRepository
    {
        public BookingPostRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<BookingPost>> GetByUserIdAsync(Guid userId)
        {
            return await _context.BookingPosts.Where(bp => bp.BookerId == userId).ToListAsync();
        }
    }
}
