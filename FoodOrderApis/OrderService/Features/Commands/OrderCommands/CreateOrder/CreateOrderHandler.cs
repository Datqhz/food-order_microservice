using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderCommands.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateOrderHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateOrderResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new CreateOrderValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            var payload = request.Payload;
            var newOrder = new Order
            {
                OrderStatus = 1,
                EaterId = payload.EaterId,
                MerchantId = payload.MerchantId,
                OrderedDate = DateTime.Now
            };
            var addResult = await _unitOfRepository.Order.Add(newOrder);
            if (addResult is null)
            {
                response.StatusText = "Order could not be added";
            }
            else
            {
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.Created;
                response.StatusText = "Order added";
            }
            return response;
        }
        catch (Exception ex)
        {
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
