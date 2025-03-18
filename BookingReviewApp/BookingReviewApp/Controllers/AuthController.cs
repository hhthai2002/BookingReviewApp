using BookingReviewApp.Dtos;
using BookingReviewApp.Enums;
using BookingReviewApp.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookingReviewApp.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly EmailService _emailService;

        public AuthController(IUserRepository userRepository, IConfiguration configuration, EmailService emailService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _emailService = emailService;
        }

        // REGISTER USER API
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            if (await _userRepository.ExistsAsync(u => u.Email == registerDto.Email))
                return BadRequest("Email has existed!");

            if (await _userRepository.ExistsAsync(u => u.Username == registerDto.Username))
                return BadRequest("Username has existed!");

            var hashedPassword = HashPassword(registerDto.Password);
            var verificationToken = Guid.NewGuid().ToString();

            var user = new User
            {
                Id = Guid.NewGuid(),
                FullName = registerDto.FullName,
                Email = registerDto.Email,
                Username = registerDto.Username,
                BankAccount = registerDto.BankAccount,
                PasswordHash = hashedPassword,
                Role = Role.USER,
                WalletBalance = 0,
                VerificationToken = verificationToken,
                IsVerified = false
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Gửi email xác thực
            var verificationLink = $"http://localhost:5085/api/auth/verify-account?token={verificationToken}";
            var emailBody = await _emailService.GetVerificationEmailAsync(verificationLink);

            await _emailService.SendEmailAsync(user.Email, "Xác nhận tài khoản", emailBody);

            return Ok(new { message = "Register Successfully! Please check your email to verify your account." });
        }

        // VERIFY ACCOUNT API
        [HttpGet("verify-account")]
        public async Task<IActionResult> VerifyAccount([FromQuery] string token)
        {
            var user = await _userRepository.GetUserByVerificationTokenAsync(token);
            if (user == null)
                return NotFound("Invalid verification token!");

            user.IsVerified = true;
            user.VerificationToken = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new { message = "Account verified successfully!" });
        }


        // LOGIN API
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(loginDto.Email);
            if (user == null || !VerifyPassword(loginDto.Password, user.PasswordHash))
                return Unauthorized("Email or password is incorrect!");

            if (!user.IsVerified)
                return Unauthorized("Account is not verified! Please check your email to verify your account.");

            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    FullName = user.FullName,
                    Email = user.Email,
                    Username = user.Username,
                    WalletBalance = user.WalletBalance,
                    Role = user.Role.ToString()
                }
            });
        }

        // LOGOUT API
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                if (string.IsNullOrEmpty(token))
                {
                    return Unauthorized("No token found!");
                }

                var cache = HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
                await cache.SetStringAsync(token, "revoked", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(3) // token expires in 3 hours
                });

                return Ok(new { message = "Logout successful!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Server Error", error = ex.Message });
            }
        }

        // GET USER PROFILE API
        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound("No account found!");

            var userDto = new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Username = user.Username,
                WalletBalance = user.WalletBalance,
                Role = user.Role.ToString()
            };

            return Ok(userDto);
        }

        // UPDATE USER PROFILE API
        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto updateProfileDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound("No account found!");

            user.FullName = updateProfileDto.FullName;
            user.Username = updateProfileDto.Username;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();
            return Ok(new { message = "Information updated successfully!" });
        }

        // CHANGE PASSWORD API
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized();

            var user = await _userRepository.GetByIdAsync(Guid.Parse(userId));
            if (user == null)
                return NotFound("No account found!");

            if (!VerifyPassword(changePasswordDto.OldPassword, user.PasswordHash))
                return BadRequest("Old password is incorrect!");

            user.PasswordHash = HashPassword(changePasswordDto.NewPassword);
            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new { message = "Password changed successfully!" });
        }

        // FORGOT PASSWORD API
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto forgotPasswordDto)
        {
            var user = await _userRepository.GetUserByEmailAsync(forgotPasswordDto.Email);
            if (user == null)
                return NotFound("Email does not exist!");

            // create reset token
            var resetToken = Guid.NewGuid().ToString();
            user.PasswordResetToken = resetToken;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddMinutes(15);

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            // send email
            var resetLink = $"http://localhost:5173/reset-password?token={resetToken}";
            var emailBody = await _emailService.GetResetPasswordEmailAsync(resetLink);

            await _emailService.SendEmailAsync(user.Email, "Reset Password", emailBody);

            return Ok(new { message = "Password reset email has been sent!" });
        }

        // RESET PASSWORD API
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            // check if model state is valid
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userRepository.GetUserByResetTokenAsync(resetPasswordDto.Token);
            if (user == null)
                return NotFound(new { message = "Invalid token!" });

            if (user.PasswordResetTokenExpiry < DateTime.UtcNow)
                return BadRequest(new { message = "Token has expired!" });

            // reset password
            user.PasswordHash = HashPassword(resetPasswordDto.NewPassword);
            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;

            _userRepository.Update(user);
            await _userRepository.SaveChangesAsync();

            return Ok(new { message = "Password reset successfully!" });
        }


        // ENCRIPT PASSWORD WITH SHA-256
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }

        // CHECK PASSWORD`
        private bool VerifyPassword(string password, string storedHash)
        {
            var hash = HashPassword(password);
            return hash == storedHash;
        }

        // GENERATE JWT TOKEN
        private string GenerateJwtToken(User user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
