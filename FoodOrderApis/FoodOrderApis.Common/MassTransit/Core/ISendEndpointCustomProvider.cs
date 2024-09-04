using MassTransit;

namespace FoodOrderApis.Common.MassTransit.Core;

public interface ISendEndpointCustomProvider : ISendEndpointProvider
{
    Task SendMessage<T>(object message, CancellationToken cancellationToken, string queueName) where T : class;
}
