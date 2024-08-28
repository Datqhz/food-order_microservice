using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Models.Configurations;

namespace OrderService.Data.Context;

public class OrderDbContext : DbContext
{
    public DbSet<Food> Foods { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetail> OrderDetails { get; set; }
    public OrderDbContext(){}
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("order");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new FoodConfiguration());
        modelBuilder.ApplyConfiguration(new OrderConfiguration());
        modelBuilder.ApplyConfiguration(new OrderDetailConfiguration());
    }
}
