using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MySocialMedia.Configs;
using MySocialMedia.Models.Users;
using System.Reflection.Emit;

namespace MySocialMedia.Models.UoW;

public class ApplicationDbContext : IdentityDbContext<User>
{
    public DbSet<Friend> Friends { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new FriendConfiguration());
        builder.ApplyConfiguration(new MessageConfiguration());

        builder.Entity<Message>()  //при запуске выскакивала ошибка про множественные циклы и нужно было выбрать каскадное удаление или обновление
            .HasOne(m => m.Sender)
            .WithMany()
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Friend>()  //при запуске выскакивала ошибка про множественные циклы и нужно было выбрать каскадное удаление или обновление
           .HasOne(m => m.CurrentFriend)
           .WithMany()
           .HasForeignKey(m => m.CurrentFriendId)
           .OnDelete(DeleteBehavior.Restrict);
    }
}
