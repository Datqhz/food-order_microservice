using AuthServer.Data.Models;
using AuthServer.Data.Seeders;
using Microsoft.AspNetCore.Identity;
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
                new ApiResource {Id = 3, Name = "OrderService", DisplayName = "Order Service"},
                new ApiResource {Id = 2, Name = "FoodService", DisplayName = "Food Service"}
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
            new Client{Id = 1, ClientId = "Mobile", ClientName = "Mobile app"}
        );
        modelBuilder.Entity<ClientGrantType>().HasData(
            new ClientGrantType{Id = 1, GrantType = "password", ClientId = 1},
            new ClientGrantType{Id = 2, GrantType = "code", ClientId = 1},
            new ClientGrantType{Id = 3, GrantType = "client_credentials", ClientId = 1}
        );
        modelBuilder.Entity<ClientScope>().HasData(
            new ClientScope{Id = 1, Scope = "food.read", ClientId = 1},
            new ClientScope{Id = 9, Scope = "food.write", ClientId = 1},
            new ClientScope{Id = 2, Scope = "customer.read", ClientId = 1},
            new ClientScope{Id = 3, Scope = "customer.write", ClientId = 1},
            new ClientScope{Id = 4, Scope = "order.read", ClientId = 1},
            new ClientScope{Id = 5, Scope = "order.write", ClientId = 1}
        );
        modelBuilder.Entity<ClientSecret>().HasData(
            new ClientSecret{Id = 1, ClientId = 1, SecretName = "mobile"}
        );
        List<string> guids = Enumerable.Range(1, 3)
            .Select(_ => Guid.NewGuid().ToString())
            .ToList();
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole{Id = guids[0], Name = "EATER", NormalizedName = "EATER" }, 
            new IdentityRole{Id = guids[1], Name = "MERCHANT", NormalizedName = "MERCHANT"},
            new IdentityRole{Id = guids[2], Name = "ADMIN", NormalizedName = "ADMIN"}
        );
        
        modelBuilder.Entity<RolePermission>().HasData(
            new RolePermission {Id = 1, Permission = "customer.read", Role = "EATER"},
            new RolePermission {Id = 2, Permission = "customer.write", Role = "EATER"},
            new RolePermission {Id = 3, Permission = "customer.read", Role = "MERCHANT"},
            new RolePermission {Id = 4, Permission = "customer.write", Role = "MERCHANT"},
            new RolePermission {Id = 5, Permission = "customer.read", Role = "ADMIN"},
            new RolePermission {Id = 6, Permission = "customer.write", Role = "ADMIN"},
            new RolePermission {Id = 7, Permission = "food.read", Role = "EATER"},
            new RolePermission {Id = 8, Permission = "food.read", Role = "MERCHANT"},
            new RolePermission {Id = 9, Permission = "food.write", Role = "MERCHANT"},
            new RolePermission {Id = 10, Permission = "food.read", Role = "ADMIN"},
            new RolePermission {Id = 11, Permission = "order.read", Role = "EATER"},
            new RolePermission {Id = 12, Permission = "order.write", Role = "EATER"},
            new RolePermission {Id = 13, Permission = "order.read", Role = "MERCHANT"},
            new RolePermission {Id = 14, Permission = "order.write", Role = "MERCHANT"},
            new RolePermission {Id = 15, Permission = "order.read", Role = "ADMIN"}
        );
    }
}
