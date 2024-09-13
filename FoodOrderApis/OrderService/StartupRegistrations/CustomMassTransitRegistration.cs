using FoodOrderApis.Common.MassTransit.Extensions;
using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;
using OrderService.Consumers;
using RabbitMQ.Client;

namespace OrderService.StartupRegistrations;

public static class CustomMassTransitRegistration
{
    public static IServiceCollection AddCustomMassTransitRegistration(this IServiceCollection services)
    {
        services.AddMassTransitRegistration((ctx, cfg) =>
        {
            cfg.ReceiveEndpoint($"order-{new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo))}", e =>
            {
                e.ConfigureConsumeTopology = false;
                e.Bind(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo)), x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });
                e.ConfigureConsumer<CreateUserConsumer>(ctx);
            });
            
            cfg.ReceiveEndpoint($"order-{new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateUserInfo))}", e =>
            {
                e.ConfigureConsumeTopology = false;
                e.Bind(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateUserInfo)), x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });
                e.ConfigureConsumer<UpdateUserConsumer>(ctx);
            });
            cfg.ReceiveEndpoint(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateFood)), e =>
            {
                e.ConfigureConsumeTopology = false;
                e.ConfigureConsumer<CreateFoodConsumer>(ctx);
            });
            cfg.ReceiveEndpoint(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateFood)), e =>
            {
                e.ConfigureConsumeTopology = false;
                e.ConfigureConsumer<UpdateFoodConsumer>(ctx);
            });
            cfg.ReceiveEndpoint(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(DeleteFood)), e =>
            {
                e.ConfigureConsumeTopology = false;
                e.ConfigureConsumer < DeleteFoodConsumer>(ctx);
            });
        });
        return services;
    }
}
