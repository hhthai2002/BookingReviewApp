using BookingReviewApp.Entities;
using System.ComponentModel.DataAnnotations;

public class Post
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required, MaxLength(500)]
    public string Content { get; set; }

    public List<string> ImageUrls { get; set; } = new List<string>();

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public Guid UserId { get; set; }
    public virtual User User { get; set; }

    public virtual ICollection<PostLike> Likes { get; set; }
    public virtual ICollection<PostComment> Comments { get; set; }
    public virtual ICollection<PostShare> Shares { get; set; }
}
