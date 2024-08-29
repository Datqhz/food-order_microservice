using System.Reflection;
using System.Text;
using AuthServer.Core;
using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthServer.Extensions;

public class ServiceExtensions
{
    
    private readonly IConfiguration _configuration;

    public ServiceExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void AddAuthenticationSettings(IServiceCollection services)
    {
        services.AddMemoryCache();
        services.AddTransient(typeof(IdentityServer4.Services.ICache<>), typeof(IdentityServer4.Services.DefaultCache<>));
        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<AuthDbContext>()
            .AddDefaultTokenProviders();
        services.AddIdentityServer()
            .AddDeveloperSigningCredential()
            .AddClientStoreCache<ClientStore>() 
            .AddResourceStoreCache<ResourceStore>() 
            .AddAspNetIdentity<User>();
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
        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (currentEnv == "Development")
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context); // Auto configure endpoint for consumers
                });
            });
        }
        else
        {
            services.AddMassTransit(x =>
            {
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.SetKebabCaseEndpointNameFormatter();
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq","/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context); // Auto configure endpoint for consumers
                });
            });
        }
    }

    public void ConfigureDbContext(IServiceCollection services)
    {
        var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        services.AddDbContext<AuthDbContext>(options => options.UseNpgsql(connectionString,
            o => { o.EnableRetryOnFailure();}));
        /*services.AddDbContext<AuthDbContext>();*/
    }

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IUnitOfRepository, UnitOfRepository>();
    }
    
}

