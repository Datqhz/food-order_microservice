using CustomerService.Data.Models;
using CustomerService.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data.Context;

public class CustomerDbContext : DbContext
{
    public DbSet<UserInfo> Users { get; set; }
    public CustomerDbContext(){}
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("customer");
        modelBuilder.ApplyConfiguration(new UserInfoConfiguration());
    }
}