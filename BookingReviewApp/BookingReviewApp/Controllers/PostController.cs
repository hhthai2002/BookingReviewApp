using BookingReviewApp.Dtos;
using BookingReviewApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookingReviewApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepository;
        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        // Get all posts
        [HttpGet]
        public async Task<IActionResult> GetPosts()
        {
            var posts = await _postRepository.GetAllAsync();
            return Ok(posts);
        }

        // Get post by current user
        [Authorize]
        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated");

            var posts = await _postRepository.GetPostsByUserIdAsync(Guid.Parse(userId));
            return Ok(posts);
        }


        // Get post by id
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost(Guid id)
        {
            var post = await _postRepository.GetByIdAsync(id);
            return Ok(post);
        }

        // Get posts by user id
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPostsByUserId(Guid userId)
        {
            var posts = await _postRepository.GetPostsByUserIdAsync(userId);
            return Ok(posts);
        }

        // Create post
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromForm] PostDto postDto, [FromForm] List<IFormFile> imageFiles)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized("User not authenticated");

            if (string.IsNullOrWhiteSpace(postDto.Content) && imageFiles.Count == 0)
                return BadRequest("Content or at least one image is required.");

            List<string> imageUrls = new List<string>();
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads");

            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);

            foreach (var file in imageFiles)
            {
                var uniqueFileName = $"{Guid.NewGuid()}_{file.FileName}";
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                imageUrls.Add($"/uploads/{uniqueFileName}");
            }

            var post = new Post
            {
                Id = Guid.NewGuid(),
                Content = postDto.Content,
                ImageUrls = imageUrls,
                CreatedAt = DateTime.UtcNow,
                UserId = Guid.Parse(userId)
            };

            await _postRepository.AddAsync(post);
            await _postRepository.SaveChangesAsync();

            return Ok(post);
        }

    }
}
