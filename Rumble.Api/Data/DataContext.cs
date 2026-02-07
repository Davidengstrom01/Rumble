using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Rumble.Api.Models;

namespace Rumble.Api.Data;

public class DataContext : IdentityDbContext<AppUser>
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Recipe> Recipes { get; set; }
    public DbSet<UserConnection> UserConnections { get; set; }
    public DbSet<Swipe> Swipes { get; set; }
    public DbSet<Match> Matches { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserConnection>()
            .HasOne(uc => uc.Requester)
            .WithMany(u => u.SentRequests)
            .HasForeignKey(uc => uc.RequesterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<UserConnection>()
            .HasOne(uc => uc.Recipient)
            .WithMany(u => u.ReceivedRequests)
            .HasForeignKey(uc => uc.RecipientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

