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
}

