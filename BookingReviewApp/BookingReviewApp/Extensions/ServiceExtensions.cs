using BookingReviewApp.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace BookingReviewApp.Extensions
{
    public static class ServiceExtensions
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IBookingPostRepository, BookingPostRepository>();
            services.AddScoped<IReviewRepository, ReviewRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
        }
    }
}
