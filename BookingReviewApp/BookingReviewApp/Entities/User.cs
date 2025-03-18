using BookingReviewApp.Entities;
using BookingReviewApp.Enums;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string FullName { get; set; }

    [Required, EmailAddress, MaxLength(100)]
    public string Email { get; set; }

    [Required, MaxLength(50)]
    public string Username { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public Role Role { get; set; } = Role.USER;

    [Range(0, double.MaxValue)]
    public decimal WalletBalance { get; set; } = 0;

    [MaxLength(50)]
    public string BankAccount { get; set; }
    public string? VerificationToken { get; set; }
    public bool IsVerified { get; set; } = false;

    public string? PasswordResetToken { get; set; }
    public DateTime? PasswordResetTokenExpiry { get; set; }

    public virtual ICollection<BookingPost> BookingPosts { get; set; }
    public virtual ICollection<Review> Reviews { get; set; }
    public virtual ICollection<Transaction> Transactions { get; set; }
    public virtual ICollection<Achievement> Achievements { get; set; }
    public virtual ICollection<Post> Posts { get; set; }
    public virtual ICollection<PostComment> Comments { get; set; }
    public virtual ICollection<PostLike> Likes { get; set; }
    public virtual ICollection<PostShare> Shares { get; set; }
}
