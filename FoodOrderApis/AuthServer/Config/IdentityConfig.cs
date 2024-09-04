using System.Runtime.InteropServices;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer.Config;

public static class IdentityConfig
{
    public static IEnumerable<ApiResource> ApiResources => new List<ApiResource>()
    {
        new ApiResource("AService", "AService"),
        new ApiResource("BService", "BService"),
        new ApiResource("CService", "CService"),
    };

    public static IEnumerable<Client> Clients => new List<Client>()
    {
        new Client
        {
            ClientId = "client",
            ClientSecrets = new List<Secret>(){new Secret("secret".Sha256())},
            AllowedGrantTypes = {"code", "password", "client_credentials"},
            AllowedScopes = {"a", "b"}
        },
    };
    public static IEnumerable<ApiScope> Scopes => new List<ApiScope>()
    {
        new ApiScope
        {
            Name = "a",
            DisplayName = "a",
        },
        new ApiScope
        {
            Name = "b",
            DisplayName = "b",
        },
    };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };
    
    public static List<TestUser> Users
        =>
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