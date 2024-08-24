using CustomerService.Data.Models;
using CustomerService.Data.Models.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data.Context;

public class CustomerDbContext : DbContext
{
    DbSet<Customer> Customers { get; set; }
    DbSet<Account> Accounts { get; set; }
    public CustomerDbContext(){}
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new CustomerConfiguration());
        modelBuilder.ApplyConfiguration(new AccountConfiguration());
    }
}