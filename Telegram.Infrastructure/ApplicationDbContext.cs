using Microsoft.EntityFrameworkCore;
using Telegram.Domain.Entities;

namespace Telegram.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserChat>(uc =>
            {
                uc.HasKey(x => x.Id);
                uc.HasIndex(x => new { x.UserId, x.ChatId }).IsUnique(true);
                uc.HasMany(x => x.Folders).WithOne(x => x.UserChat).HasForeignKey(x => x.UserChatId).OnDelete(DeleteBehavior.Cascade);
                uc.HasOne(x => x.User).WithMany(x => x.UserChats).HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                uc.HasOne(x => x.Chat).WithMany(x => x.UserChats).HasForeignKey(x => x.ChatId).OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<BlockedUser>(bu =>
            {
                bu.HasKey(x => x.Id);
                bu.HasIndex(x => new { x.BlockedUserId, x.BlockedByUserId }).IsUnique(true);
                bu.HasOne(x => x.User)
                    .WithMany(x => x.BlockedUsers)
                    .HasForeignKey(x => x.BlockedByUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<User>(user =>
            {
                user.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Folder>(folder =>
            {
                folder.HasKey(x => x.Id);
            });

            modelBuilder.Entity<Chat>(chat =>
            {
                chat.HasKey(x => x.Id);
                chat.HasMany(x => x.Messages).WithOne(x => x.Chat).HasForeignKey(x => x.ChatId);
                chat.Property(x => x.ChatType).HasConversion<string>();
            });

            modelBuilder.Entity<Message>(msg =>
            {
                msg.HasKey(x => x.Id);
                msg.Property(x => x.Type).HasConversion<string>();
            });

            modelBuilder.Entity<OutboxMessage>(msg =>
            {
                msg.HasKey(x => x.Id);
            });
            modelBuilder.Entity<OutboxMessageConsumer>(msg =>
            {
                msg.HasKey(x => x.Id);
            });

            base.OnModelCreating(modelBuilder);
        }

    }
}
