using IdentityModel.Client;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer.Config
{
    public static class AuthServerConfig
    {

        public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>
        {
            new ApiResource("api1", "My API"),
            new ApiResource("api2", "My api 2")
        };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("api.read", "Can read data from API"),
                new ApiScope("api.write", "Can write data via API")
            };

        public static IEnumerable<Client> Clients =>

            new List<Client>
            {
                new Client
                {
                    ClientId = "client1",
                    
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = 
                    {
                        new Secret("secret1".Sha256())
                    },
                    AllowedScopes = { "api.read" }
                },
                new Client
                {
                    ClientId = "client2",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("secret2".Sha256())
                    },
                    AllowedScopes = { "api.write" }
                }
            };

        public static List<TestUser> UserTests =>
            new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "alice",
                    Password = "password"
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "bob",
                    Password = "password"
                }
            };
    }
}

