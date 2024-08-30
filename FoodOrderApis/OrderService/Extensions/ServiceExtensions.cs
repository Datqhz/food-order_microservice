using System.Reflection;
using FoodOrderApis.Common.Helpers;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OrderService.Consumers;
using OrderService.Data.Context;
using OrderService.Repositories;
using RabbitMQ.Client;

namespace OrderService.Extensions;

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
        services.AddDbContext<OrderDbContext>(options => options.UseNpgsql(connectionString,
            o => { o.EnableRetryOnFailure();}));
        //services.AddDbContext<OrderDbContext>();
    }

    public void ConfigureDependencyInjection(IServiceCollection services)
    {
        services.AddScoped<IUnitOfRepository, UnitOfRepository>();
        services.AddLogging();
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
                x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("order",false));
                x.SetKebabCaseEndpointNameFormatter();
                //x.AddConsumer<CreateFoodConsumer>();
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
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
                    cfg.ReceiveEndpoint("create-food", e =>
                    {
                        e.ConfigureConsumer<CreateFoodConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order_service_update_food", e =>
                    {
                        e.ConfigureConsumer<UpdateFoodConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order_service_test", e =>
                    {
                        e.ConfigureConsumer<CreateUserConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order_service_create_user", e =>
                    {
                        e.ConfigureConsumer<CreateUserConsumer>(context);
                    });
                    cfg.ReceiveEndpoint("order_service_update_user", e =>
                    {
                        e.ConfigureConsumer<CreateUserConsumer>(context);
                    });
                    // Auto configure endpoint for consumers
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
    }
}
