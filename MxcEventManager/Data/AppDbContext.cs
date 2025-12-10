using Microsoft.EntityFrameworkCore;
using MxcEventManager.Models;

namespace MxcEventManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }

    public DbSet<Event> Events { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Country> Countries { get; set; }
}
