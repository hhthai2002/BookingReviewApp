using BookingReviewApp.Dtos;
using BookingReviewApp.Entities;
using BookingReviewApp.Repositories;
using BookingReviewApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingPostRepository _bookPostRepository;
        private readonly LocationService _locationService;

        public BookingController(IBookingPostRepository bookPostRepository, LocationService locationService)
        {
            _bookPostRepository = bookPostRepository;
            _locationService = locationService;
        }

        // create post
        [HttpPost]
        public async Task<IActionResult> CreateBookPost([FromBody] BookingPostDto bookingPostDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated");

            var bookingPost = new BookingPost
            {
                Id = Guid.NewGuid(),
                Title = bookingPostDto.Title,
                Description = bookingPostDto.Description,
                Price = bookingPostDto.Price,
                Location = bookingPostDto.Location,
                Latitude = bookingPostDto.Latitude,
                Longitude = bookingPostDto.Longitude,
                BookerId = Guid.Parse(userId)
            };

            await _bookPostRepository.AddAsync(bookingPost);
            await _bookPostRepository.SaveChangesAsync();

            return Ok(bookingPost);
        }


        // get nearby posts
        [HttpGet("nearby")]
        public async Task<IActionResult> GetNearbyBookPosts([FromQuery] double userLat, [FromQuery] double userLng, [FromQuery] double radiusKm = 5)
        {
            var allPosts = await _bookPostRepository.GetAllAsync();

            var nearbyPosts = allPosts
                .Where(post => _locationService.CalculateDistance(userLat, userLng, post.Latitude, post.Longitude) <= radiusKm)
                .Select(post => new BookingPostDto
                {
                    Title = post.Title,
                    Description = post.Description,
                    Price = post.Price,
                    Location = post.Location,
                    Latitude = post.Latitude,
                    Longitude = post.Longitude,
                    BookerId = post.BookerId
                })
                .ToList();

            return Ok(nearbyPosts);
        }

    }
}
