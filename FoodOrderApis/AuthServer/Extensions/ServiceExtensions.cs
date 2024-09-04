using System.Reflection;
using System.Security.Claims;
using System.Text;
using AuthServer.Config;
using AuthServer.Core;
using AuthServer.Data.Context;
using AuthServer.Data.Models;
using AuthServer.Repositories;
using FoodOrderApis.Common.Helpers;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using IdentityServer4.Test;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace AuthServer.Extensions;

public class ServiceExtensions
{
    
    private readonly IConfiguration _configuration;

    public ServiceExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureIdentityServer(IServiceCollection services)
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
        /*.AddInMemoryIdentityResources(IdentityConfig.IdentityResources)
        .AddInMemoryApiResources(IdentityConfig.ApiResources)
        .AddInMemoryApiScopes(IdentityConfig.Scopes)
        .AddInMemoryClients(IdentityConfig.Clients)
        .AddTestUsers(IdentityConfig.Users);*/
    }
    public void AddAuthenticationSettings(IServiceCollection services)
    {
        services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5092";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = EncodeHelper.CreateRsaKey()
                };
            });
        services.AddTransient<ClaimsPrincipal>(provider =>
            provider.GetService<IHttpContextAccessor>().HttpContext?.User);
        services.AddAuthorization(options =>
        {
            options.AddPolicy("ApiScope", policy => policy.RequireClaim("scope"));
        });
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
    
    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthApi", Version = "v1" });
            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });
    }
}

