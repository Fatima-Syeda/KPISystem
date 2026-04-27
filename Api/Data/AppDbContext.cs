using Microsoft.EntityFrameworkCore;
using Api.Models;

namespace Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<Event> Events => Set<Event>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
}