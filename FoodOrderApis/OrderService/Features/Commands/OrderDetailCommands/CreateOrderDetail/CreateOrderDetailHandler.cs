using FoodOrderApis.Common.Helpers;
using MediatR;
using OrderService.Data.Models;
using OrderService.Data.Responses;
using OrderService.Repositories;

namespace OrderService.Features.Commands.OrderDetailCommands.CreateOrderDetail;

public class CreateOrderDetailHandler : IRequestHandler<CreateOrderDetailCommand, CreateOrderDetailResponse >
{
    private readonly IUnitOfRepository _unitOfRepository;

    public CreateOrderDetailHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<CreateOrderDetailResponse> Handle(CreateOrderDetailCommand request, CancellationToken cancellationToken)
    {
        var response = new CreateOrderDetailResponse(){StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new CreateOrderDetailValidator();
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }

            var payload = request.Payload;
            var orderDetail = new OrderDetail
            {
                FoodId = payload.FoodId,
                OrderId = payload.OrderId,
                Quantity = payload.Quantity,
                Price = payload.Price,
            };
            var createResult = await _unitOfRepository.OrderDetail.Add(orderDetail);
            if (createResult != null)
            {
                await _unitOfRepository.CompleteAsync();
                response.StatusCode = (int)ResponseStatusCode.Created;
                response.StatusText = "Order Detail Created";
            }
            else
            {
                response.StatusText = "Order Detail Not Created";
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