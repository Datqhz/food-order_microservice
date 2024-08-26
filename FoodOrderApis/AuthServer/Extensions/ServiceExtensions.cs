using System.Reflection;
using System.Text;
using AuthServer.Config;
using FoodOrderApis.Common.Helpers;
using IdentityModel;
using MassTransit;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Extensions;

public class ServiceExtensions
{
    public void AddAuthenticationSettings(IServiceCollection services)
    {
        services.AddIdentityServer()
            /*.AddSigningCredential(EncodeHelper.CreateSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256)*/
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

    public void AddMassTransitWithRabbitMq(IServiceCollection services)
    {
        services.AddMassTransit(x =>
        {
            x.AddConsumers(Assembly.GetEntryAssembly());
            x.SetKebabCaseEndpointNameFormatter();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host("rabbitmq", "/", h =>
                {
                    h.Username("guest");
                    h.Password("guest");
                });
                cfg.ConfigureEndpoints(context); // Auto configure endpoint for consumers
            });
        });
    }
}