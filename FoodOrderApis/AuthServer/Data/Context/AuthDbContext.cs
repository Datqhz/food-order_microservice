using AuthServer.Data.Models;
using AuthServer.Data.Seeders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Context;

public class AuthDbContext : IdentityDbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<ClientGrantType> ClientGrantTypes { get; set; }
    public DbSet<ClientSecret> ClientSecrets { get; set; }
    public DbSet<ClientScope> ClientScopes { get; set; }
    public DbSet<ApiResource> ApiResources { get; set; }
    public DbSet<ApiResourceScope> ApiResourceScope { get; set; }
    public AuthDbContext(){}
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options){}
    /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql("Host=127.0.0.1; port=5432; Database=db_test; Username=myuser; Password=123456");
            //optionsBuilder.UseNpgsql("Host=db; port=5432; Database=db_test; Username=myuser; Password=123456");
            optionsBuilder.EnableSensitiveDataLogging(true);
        }
    }*/

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("auth");
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
        /*DataSeeder seeder = new DataSeeder(modelBuilder);*/
        /*seeder.InitSeedData();*/
        modelBuilder.Entity<ApiResource>().HasData(
                new ApiResource {Id = 1, Name = "CustomerService", DisplayName = "Customer Service"},
                new ApiResource {Id = 2, Name = "OrderService", DisplayName = "Order Service"},
                new ApiResource {Id = 3, Name = "FoodService", DisplayName = "Food Service"}
            );
        modelBuilder.Entity<ApiResourceScope>().HasData(
            new ApiResourceScope {Id = 1, Scope = "customer.read", ApiResourceId = 1},
            new ApiResourceScope {Id = 2, Scope = "customer.write", ApiResourceId = 1},
            new ApiResourceScope {Id = 3, Scope = "food.read", ApiResourceId = 2},
            new ApiResourceScope {Id = 4, Scope = "food.write", ApiResourceId = 2},
            new ApiResourceScope {Id = 5, Scope = "order.read", ApiResourceId = 3},
            new ApiResourceScope {Id = 6, Scope = "order.write", ApiResourceId = 3}
        );
        modelBuilder.Entity<Client>().HasData(
            new Client{Id = 1, ClientId = "Eater", ClientName = "Eater"},
            new Client{Id = 2, ClientId = "Merchant", ClientName = "Merchant"}
        );
        modelBuilder.Entity<ClientGrantType>().HasData(
            new ClientGrantType{Id = 1, GrantType = "password", ClientId = 1},
            new ClientGrantType{Id = 2, GrantType = "password", ClientId = 2}
        );
        modelBuilder.Entity<ClientScope>().HasData(
            new ClientScope{Id = 1, Scope = "food.read", ClientId = 1},
            new ClientScope{Id = 2, Scope = "customer.read", ClientId = 1},
            new ClientScope{Id = 3, Scope = "customer.write", ClientId = 1},
            new ClientScope{Id = 4, Scope = "order.read", ClientId = 1},
            new ClientScope{Id = 5, Scope = "order.write", ClientId = 1},
            new ClientScope{Id = 6, Scope = "customer.read", ClientId = 2},
            new ClientScope{Id = 7, Scope = "customer.write", ClientId = 2},
            new ClientScope{Id = 8, Scope = "food.read", ClientId = 2},
            new ClientScope{Id = 9, Scope = "food.write", ClientId = 2},
            new ClientScope{Id = 10, Scope = "order.read", ClientId = 2}
        );
        modelBuilder.Entity<ClientSecret>().HasData(
            new ClientSecret{Id = 1, ClientId = 1, SecretName = "Eater"},
            new ClientSecret{Id = 2, ClientId = 2, SecretName = "Merchant"}
        );
    }
}
