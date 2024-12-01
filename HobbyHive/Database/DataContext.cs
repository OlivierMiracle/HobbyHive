using HobbyHive.Models;
using Microsoft.EntityFrameworkCore;

namespace HobbyHive.Database;

public class DataContext : DbContext
{
    public DbSet<Activity> Activities { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    public DataContext() { }
    public DataContext(DbContextOptions options) : base(options) { } 

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseMySql(
            "Host=panel.sebastiankura.com;Port=25579;Database=oneparagraph;Username=hobbyhatch;Password=nalesnik",
            new MySqlServerVersion(new Version(10, 3, 39)));
        options.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Activity>()
            .HasMany<Category>(activity => activity.Categories)
            .WithMany();

        modelBuilder.Entity<User>()
            .HasMany<Category>(c => c.Categories)
            .WithMany();
        
        base.OnModelCreating(modelBuilder);
    }
}