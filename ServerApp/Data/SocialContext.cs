using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServerApp.Models;

namespace ServerApp.Data
{
    public class SocialContext : IdentityDbContext<User, Role, int>
    {
        public SocialContext(DbContextOptions<SocialContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

        public DbSet<Image> Images { get; set; }

        public DbSet<UserToUser> UserToUsers { get; set; }

        public DbSet<Message> Messages { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserToUser>()
                .HasKey(u => new { u.UserId, u.FollowerId });

            builder.Entity<UserToUser>()
                .HasOne(uu => uu.User)
                .WithMany(u => u.Followers)
                .HasForeignKey(uu => uu.FollowerId);

            builder.Entity<UserToUser>()
                .HasOne(uu => uu.Follower)
                .WithMany(u => u.Followings)
                .HasForeignKey(uu => uu.UserId);

            builder.Entity<Message>()
                .HasOne(m => m.Sender)
                .WithMany(u => u.MessageSent)
                .HasForeignKey(m => m.SenderId);

            builder.Entity<Message>()
                .HasOne(m => m.Recipient)
                .WithMany(u => u.MessageReceived)
                .HasForeignKey(m => m.RecipientId);
        }

    }
}