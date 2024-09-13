using MassTransit;
using MassTransit.Transports.Fabric;

namespace FoodOrderApis.Common.MassTransit.Core;

public interface ISendEndpointCustomProvider : ISendEndpointProvider
{
    Task SendMessage<T>(object message, CancellationToken cancellationToken, ExchangeType type) where T : class;
}
