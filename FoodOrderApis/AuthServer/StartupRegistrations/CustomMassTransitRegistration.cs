using FoodOrderApis.Common.MassTransit.Contracts;
using FoodOrderApis.Common.MassTransit.Extensions;
using MassTransit;
using MassTransit.Transports.Fabric;
using ExchangeType = RabbitMQ.Client.ExchangeType;

namespace AuthServer.StartupRegistrations;

public static class CustomMassTransitRegistration
{
    public static IServiceCollection AddCustomMassTransitRegistration(this IServiceCollection services)
    {
        services.AddMassTransitRegistration((ctx, cfg) =>
        {
            cfg.Message<CreateUserInfo>(x =>
            {
                x.SetEntityName(new KebabCaseEndpointNameFormatter(false).SanitizeName(nameof(CreateUserInfo)));
            });
            cfg.Publish<CreateUserInfo>(cfgPublishTopology =>
            {
                cfgPublishTopology.ExchangeType = ExchangeType.Topic;
            });
        });
        return services;
    }
}
