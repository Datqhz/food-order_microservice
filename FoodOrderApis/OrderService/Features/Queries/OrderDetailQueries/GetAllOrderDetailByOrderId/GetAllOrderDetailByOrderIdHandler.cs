using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models.Dtos;
using OrderService.Data.Responses;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailByOrderId;

public class GetAllOrderDetailByOrderIdHandler : IRequestHandler<GetAllOrderDetailByOrderIdQuery, GetAllOrderDetailByOrderIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetAllOrderDetailByOrderIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllOrderDetailByOrderIdResponse> Handle(GetAllOrderDetailByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllOrderDetailByOrderIdResponse(){StatusCode = (int)ResponseStatusCode.NoContent};
        var orderId = request.OrderId;
        try
        {
            var orderDetails = await _unitOfRepository.OrderDetail
                .Where(od => od.OrderId == orderId)
                .Include(od => od.Food).Select(_ => new OrderDetailDto
                {
                    Id = _.Id,
                    OrderId = _.OrderId,
                    Food = _.Food.AsDto()
                }).AsNoTracking().ToListAsync(cancellationToken);
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all order details by order id successfully";
            response.Data = orderDetails;
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
