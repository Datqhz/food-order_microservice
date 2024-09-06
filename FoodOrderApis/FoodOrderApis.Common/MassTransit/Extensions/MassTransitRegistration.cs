using System.Reflection;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodOrderApis.Common.MassTransit.Extensions;

public static class MassTransitRegistration
{
    public static IServiceCollection AddMassTransitRegistration(this IServiceCollection services, string? prefix)
    {
        var currentEnv = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (currentEnv == "Development")
        {
            services.AddMassTransit(x =>
            {
                if (prefix != null)
                {
                    Console.WriteLine($"prefix: {prefix}");
                    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix, false));
                }
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.PrefetchCount = 10; // Limit the number of message preloaded
                    //cfg.UseMessageRetry(r => r.Interval(2, TimeSpan.FromDays(10))); // Retry 3 times
                    cfg.UseDelayedRedelivery(r => r.Intervals(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5))); // 
                    // Auto configure endpoint for consumers
                    cfg.ConfigureEndpoints(context);
                });
            });
        }
        else
        {
            services.AddMassTransit(x =>
            {
                if (prefix != null)
                {
                    Console.WriteLine($"prefix: {prefix}");
                    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix, false));
                }
                x.SetKebabCaseEndpointNameFormatter();
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("rabbitmq","/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });
                    cfg.ConfigureEndpoints(context);
                    // Auto configure endpoint for consumers
                });
            });
        }

        services.AddScoped<ISendEndpointCustomProvider, SendEndpointCustomProvider>();
        return services;
    }
}
