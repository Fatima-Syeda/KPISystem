using Microsoft.EntityFrameworkCore;
using Worker.Models;

namespace Worker.Data;

public class AppDbContext : DbContext
{
    public DbSet<EventRecord> Events => Set<EventRecord>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}