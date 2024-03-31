using LocaSubs.Models;
using Microsoft.EntityFrameworkCore;

namespace LocaSubs.DataAccess;

public class LocaSubsDbContext : DbContext
{
    private readonly string _dbPath;

    public DbSet<User>? Users { get; set; }
    public DbSet<Subscription>? Subscriptions { get; set; }
    
    public LocaSubsDbContext(DbContextOptions<LocaSubsDbContext> options)
        : base(options)
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        _dbPath = Path.Join(path, "locasubs.db");
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite($"Data Source={_dbPath}");
    }
}
