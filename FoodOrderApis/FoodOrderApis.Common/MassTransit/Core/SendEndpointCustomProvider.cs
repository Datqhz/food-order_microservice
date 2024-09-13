using MassTransit;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using ExchangeType = MassTransit.Transports.Fabric.ExchangeType;

namespace FoodOrderApis.Common.MassTransit.Core;

public class SendEndpointCustomProvider : ISendEndpointCustomProvider
{
    
    private readonly IBusControl _busControl;
    private readonly ILogger<SendEndpointCustomProvider> _logger;

    public SendEndpointCustomProvider(IBusControl busControl, ILogger<SendEndpointCustomProvider> logger)
    {
        _busControl = busControl;
        _logger = logger;
    }
    public ConnectHandle ConnectSendObserver(ISendObserver observer)
    {
        return _busControl.ConnectSendObserver(observer);
    }

    public Task<ISendEndpoint> GetSendEndpoint(Uri address)
    {
        return _busControl.GetSendEndpoint(address);
    }

    public async Task SendMessage<T>(object message, CancellationToken cancellationToken, ExchangeType type) where T : class
    {
        const string funcName = $"{nameof(SendEndpointCustomProvider)} {nameof(SendMessage)} =>";
        
        try
        {
            _logger.LogInformation($"{funcName} is called ...");
            var target = new KebabCaseEndpointNameFormatter(false).SanitizeName(typeof(T).Name);
            if (type is ExchangeType.Topic)
            {
                _logger.LogInformation($"{funcName} Exchange: {target}");
                await _busControl.Publish<T>(message, cancellationToken);
            }
            else
            {
                _logger.LogInformation($"{funcName} queue: {target}");
                var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"queue:{target}"));
                await sendEndpoint.Send<T>(message, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
