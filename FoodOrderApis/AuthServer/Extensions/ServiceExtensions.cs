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

    public void AddCors(IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAllOrigins",
                builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
            );
        });
    }
    public void ConfigureMediator(IServiceCollection services)
    {
        services.AddMediatR(configure => 
            configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }
}