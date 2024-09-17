using System.Reflection;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Core;
using MassTransit;
using MassTransit.Metadata;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace FoodOrderApis.Common.MassTransit.Extensions;

public static class MassTransitRegistration
{
    public static IServiceCollection AddMassTransitRegistration(this IServiceCollection services, Action<IBusRegistrationContext,  IRabbitMqBusFactoryConfigurator>? registrationConfigure = null)
    {
            services.AddMassTransit(x =>
            {
                /*if (prefix != null)
                {
                    Console.WriteLine($"prefix: {prefix}");
                    x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter(prefix, false));
                }*/
                x.SetKebabCaseEndpointNameFormatter(); 
                x.AddConsumers(Assembly.GetEntryAssembly());
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(HostMetadataCache.IsRunningInContainer ? "rabbitmq" : "localhost", 5672,"/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });  
                    registrationConfigure?.Invoke(context, cfg);
                    cfg.ConfigureEndpoints(context);
                });
            });
       

        services.AddScoped<ISendEndpointCustomProvider, SendEndpointCustomProvider>();
        return services;
    }
}
