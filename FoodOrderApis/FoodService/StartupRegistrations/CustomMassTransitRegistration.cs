using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Extensions;
using FoodService.Consumers;
using MassTransit;
using RabbitMQ.Client;

namespace FoodService.StartupRegistrations;

public static class CustomMassTransitRegistration
{
    public static IServiceCollection AddCustomMassTransitRegistration(this IServiceCollection services)
    {
        services.AddMassTransitRegistration((ctx, cfg) =>
        {
            cfg.ReceiveEndpoint($"food-{new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo))}", e =>
            {
                e.ConfigureConsumeTopology = false;
                e.Bind(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo)), x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });
                e.ConfigureConsumer<CreateUserConsumer>(ctx);
            });
            cfg.ReceiveEndpoint($"food-{new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateUserInfo))}", e =>
            {
                e.ConfigureConsumeTopology = false;
                e.Bind(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateUserInfo)), x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });
                e.ConfigureConsumer<UpdateUserConsumer>(ctx);
            });
        });
        return services;
    }
}
