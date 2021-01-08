using Microsoft.EntityFrameworkCore;
using Udemy.NetCore5.Angular.Data.Entities;

namespace Udemy.NetCore5.Angular.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public virtual DbSet<AppUser> Users { get; set; }

        public DbSet<AppUserLike> Likes { get; set; }

        public DbSet<Message> Messages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            SetupUserLikeRelationship(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private static void SetupUserLikeRelationship(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppUserLike>()
                .HasKey(k => new {k.SourceUserId, k.LikedUserId});

            modelBuilder.Entity<AppUserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<AppUserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}