using FoodService.Data.Models;
using FoodService.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace FoodService.Data.Context;

public class FoodDbContext : DbContext
{
    public DbSet<Food> Foods { get; set; }
    public DbSet<User> Users { get; set; }
    public FoodDbContext(){}
    public FoodDbContext(DbContextOptions<FoodDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("food");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new FoodConfiguration());
    }
}
