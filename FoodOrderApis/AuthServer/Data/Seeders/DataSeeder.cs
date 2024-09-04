using AuthServer.Data.Models;
using AuthServer.Repositories;
using Microsoft.EntityFrameworkCore;

namespace AuthServer.Data.Seeders;

public class DataSeeder
{
    private readonly ModelBuilder _modelBuilder;

    public DataSeeder(ModelBuilder modelBuilder)
    {
        _modelBuilder = modelBuilder;
    }

    public void InitSeedData()
    {
            ApiResourceScopeSeedData();
            ApiResourceSeedData();
            ClientSeedData();
            ClientScopeSeedData();
            ClientSecretSeedData();
            ClientGrantTypeSeedData();
    }
    public void ApiResourceSeedData()
    {
            _modelBuilder.Entity<ApiResource>().HasData(
                new ApiResource {Id = 1, Name = "CustomerService", DisplayName = "Customer Service"},
                new ApiResource {Id = 2, Name = "OrderService", DisplayName = "Order Service"},
                new ApiResource {Id = 3, Name = "FoodService", DisplayName = "Food Service"}
            );
    }
    public void ApiResourceScopeSeedData()
    {
            _modelBuilder.Entity<ApiResourceScope>().HasData(
                    new ApiResourceScope {Id = 1, Scope = "customer.read", ApiResourceId = 1},
                    new ApiResourceScope {Id = 2, Scope = "customer.write", ApiResourceId = 1},
                    new ApiResourceScope {Id = 3, Scope = "food.read", ApiResourceId = 2},
                    new ApiResourceScope {Id = 4, Scope = "food.write", ApiResourceId = 2},
                    new ApiResourceScope {Id = 5, Scope = "order.read", ApiResourceId = 3},
                    new ApiResourceScope {Id = 6, Scope = "order.write", ApiResourceId = 3}
            );
    }
    public void ClientSeedData()
    {
            _modelBuilder.Entity<Client>().HasData(
                    new Client{Id = 1, ClientId = "Eater", ClientName = "Eater"},
                    new Client{Id = 2, ClientId = "Merchant", ClientName = "Merchant"}
            );
    }
    public void ClientGrantTypeSeedData()
    {
            _modelBuilder.Entity<ClientGrantType>().HasData(
                    new ClientGrantType{Id = 1, GrantType = "password", ClientId = 1},
                    new ClientGrantType{Id = 2, GrantType = "code", ClientId = 1},
                    new ClientGrantType{Id = 3, GrantType = "client_credentials", ClientId = 1},
                    new ClientGrantType{Id = 4, GrantType = "password", ClientId = 2},
                    new ClientGrantType{Id = 5, GrantType = "code", ClientId = 2},
                    new ClientGrantType{Id = 6, GrantType = "client_credentials", ClientId = 2}
            );
    }
    public void ClientScopeSeedData()
    {
            _modelBuilder.Entity<ClientScope>().HasData(
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
    }
    public void ClientSecretSeedData()
    {
            _modelBuilder.Entity<ClientSecret>().HasData(
                    new ClientSecret{Id = 1, ClientId = 1, SecretName = "Eater"},
                    new ClientSecret{Id = 2, ClientId = 2, SecretName = "Merchant"}
            );
    }
}
