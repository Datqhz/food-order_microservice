using FoodOrderApis.Common.MassTransit.Contracts;
using MassTransit;

namespace OrderService.Consumers;

public class CreateUserConsumer : IConsumer<CreateUserInfo>
{
    public async Task Consume(ConsumeContext<CreateUserInfo> context)
    {
        Console.WriteLine("CreateUserConsumer");
    }
}
