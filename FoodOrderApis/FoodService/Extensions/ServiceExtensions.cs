using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodService.Consumers;
using FoodService.Data.Context;
using FoodService.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;

namespace FoodService.Extensions;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;


public class ServiceExtensions
{
    private readonly IConfiguration _configuration;

    public ServiceExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public void AddAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5092";
                options.RequireHttpsMetadata = false;
                options.Audience = "FoodService";
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = EncodeHelper.CreateRsaKey()
                };
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
    public void ConfigureDbContext(IServiceCollection services)
    {  
        var connectionString = _configuration.GetValue<string>("DatabaseSettings:ConnectionString");
        services.AddDbContext<FoodDbContext>(options => options.UseNpgsql(connectionString,
            o => { o.EnableRetryOnFailure();}));
    }

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IUnitOfRepository, UnitOfRepository>();
    }
    
    public void AddMediatorPattern(IServiceCollection services)
    {
        services.AddMediatR(configure => 
            configure.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "FoodApi", Version = "v1" });
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

    public void AddMassTransitRabbitMq(IServiceCollection services)
    {
        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (currentEnv == "Development")
        {
            services.AddMassTransit(x =>
            {
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("food",false));
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    /*cfg.ReceiveEndpoint("food_service_create_user", e =>
                    {
                        e.ConfigureConsumer<CreateUserConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("food_service_update_user", e =>
                    {
                        e.ConfigureConsumer<UpdateUserConsumer>(context);
                    });*/
                    // Auto configure endpoint for consumers
                    cfg.ConfigureEndpoints(context);
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
                    /*cfg.Publish<CreateFood>(p =>
                    {
                        p.ExchangeType = ExchangeType.Topic;
                        p.BindAlternateExchangeQueue("create-food-exchange", "create-food-queue", cf =>
                        {
                            cf.ExchangeType = ExchangeType.Topic;
                        });
                    });*/
                    /*cfg.ReceiveEndpoint("food_service_create_user", e =>
                    {
                        e.ConfigureConsumer<CreateUserConsumer>
                        (context);
                    });
                    cfg.ReceiveEndpoint("food_service_update_user", e =>
                    {
                        e.ConfigureConsumer<UpdateUserConsumer>(context);
                    });*/
                    //cfg.ConfigureEndpoints(context);
                    // Auto configure endpoint for consumers
                });
            });
        }
        
    }

    public void AddAuthorizationSettings(IServiceCollection services)
    {
        services.AddAuthorization(option =>
        {
            option.AddPolicy("FoodRead",
                policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(claim => claim.Type == "scope" && claim.Value.Contains("food.read"))
                ));
            option.AddPolicy("FoodWrite",
                policy => policy.RequireAssertion(context =>
                    context.User.HasClaim(claim => claim.Type == "scope" && claim.Value.Contains("food.write"))
                ));
        });
    }
    public void InitializeDatabase(IApplicationBuilder app){
        using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<FoodDbContext>();

            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
        }
    }
}