using CustomerService.Consumers;
using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Extensions;
using MassTransit;
using RabbitMQ.Client;

namespace CustomerService.StartupRegistrations;

public static class CustomMassTransitRegistration
{
    public static IServiceCollection AddCustomMassTransitRegistration(this IServiceCollection services)
    {
        services.AddMassTransitRegistration((ctx, cfg) =>
        {
            cfg.ReceiveEndpoint($"customer-{new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo))}", e =>
            {
                e.ConfigureConsumeTopology = false;
                var exchangeName = new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo));
                e.Bind(exchangeName, x =>
                {
                    x.ExchangeType = ExchangeType.Topic;
                });
                e.ConfigureConsumer<CreateUserConsumer>(ctx);
            });
            cfg.ReceiveEndpoint(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(DeleteUserInfo)), e =>
            {
                e.ConfigureConsumeTopology = false;
                e.ConfigureConsumer<DeleteUserConsumer>(ctx);
            });
            
            cfg.Message<UpdateUserInfo>(x =>
            {
                x.SetEntityName(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(UpdateUserInfo)));
            });
            cfg.Publish<UpdateUserInfo>(cfgPublishTopology =>
            {
                cfgPublishTopology.ExchangeType = ExchangeType.Topic;
            });
        });
        return services;
    }
}
