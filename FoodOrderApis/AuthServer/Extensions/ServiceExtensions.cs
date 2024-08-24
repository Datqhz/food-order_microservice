using System.Reflection;
using AuthServer.Config;

namespace AuthServer.Extensions;

public class ServiceExtensions
{
    public void AddAuthenticationSettings(IServiceCollection services)
    {
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddInMemoryApiScopes(AuthServerConfig.ApiScopes)
            .AddInMemoryClients(AuthServerConfig.Clients)
            .AddTestUsers(AuthServerConfig.UserTests);
    }
    public void ConfigureMediator(IServiceCollection services)
    {
        services.AddMediatR(configure => 
            configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}