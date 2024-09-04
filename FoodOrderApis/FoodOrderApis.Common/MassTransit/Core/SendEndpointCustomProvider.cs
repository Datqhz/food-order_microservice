using MassTransit;

namespace FoodOrderApis.Common.MassTransit.Core;

public class SendEndpointCustomProvider : ISendEndpointCustomProvider
{
    
    private readonly IBusControl _busControl;

    public SendEndpointCustomProvider(IBusControl busControl)
    {
        _busControl = busControl;
    }
    public ConnectHandle ConnectSendObserver(ISendObserver observer)
    {
        return _busControl.ConnectSendObserver(observer);
    }

    public Task<ISendEndpoint> GetSendEndpoint(Uri address)
    {
        return _busControl.GetSendEndpoint(address);
    }

    public async Task SendMessage<T>(object message, CancellationToken cancellationToken, string queueName = null) where T : class
    {
        try
        {
            if (queueName is null)
            {
                _busControl.Publish<T>(message, cancellationToken);
            }
            else
            {
                var sendEndpoint = await _busControl.GetSendEndpoint(new Uri($"queue:{queueName}"));
                await sendEndpoint.Send<T>(message, cancellationToken);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
