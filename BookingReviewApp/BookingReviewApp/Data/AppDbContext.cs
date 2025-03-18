using Microsoft.EntityFrameworkCore;
using BookingReviewApp.Entities;

namespace BookingReviewApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<BookingPost> BookingPosts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Report> Reports { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostLike> PostLikes { get; set; }
        public DbSet<PostComment> PostComments { get; set; }
        public DbSet<PostShare> PostShares { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Định dạng kiểu decimal(18,2) để tránh mất dữ liệu
            modelBuilder.Entity<BookingPost>()
                .Property(bp => bp.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Transaction>()
                .Property(t => t.Amount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Review>()
                .Property(r => r.AmountPaid)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<User>()
                .Property(u => u.WalletBalance)
                .HasColumnType("decimal(18,2)");

            // Khi BookingPost bị xóa, Review cũng bị xóa
            modelBuilder.Entity<Review>()
                .HasOne(r => r.BookingPost)
                .WithMany(bp => bp.Reviews)
                .HasForeignKey(r => r.BookingPostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Khi User bị xóa, giữ nguyên Review để tránh vòng lặp
            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.ReviewerId)
                .OnDelete(DeleteBehavior.NoAction);

            // Khi Post bị xóa, toàn bộ Comment, Like, Share cũng bị xóa
            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(pc => pc.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.Post)
                .WithMany(p => p.Likes)
                .HasForeignKey(pl => pl.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PostShare>()
                .HasOne(ps => ps.Post)
                .WithMany(p => p.Shares)
                .HasForeignKey(ps => ps.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            // Khi User bị xóa, giữ nguyên Comment, Like, Share
            modelBuilder.Entity<PostComment>()
                .HasOne(pc => pc.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(pc => pc.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Giữ comment nếu User bị xóa

            modelBuilder.Entity<PostLike>()
                .HasOne(pl => pl.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(pl => pl.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Giữ Like nếu User bị xóa

            modelBuilder.Entity<PostShare>()
                .HasOne(ps => ps.User)
                .WithMany(u => u.Shares)
                .HasForeignKey(ps => ps.UserId)
                .OnDelete(DeleteBehavior.NoAction); // Giữ Share nếu User bị xóa

            base.OnModelCreating(modelBuilder);
        }
    }
}
