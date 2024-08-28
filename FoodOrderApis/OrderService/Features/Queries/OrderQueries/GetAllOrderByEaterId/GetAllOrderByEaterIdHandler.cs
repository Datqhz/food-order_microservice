using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Responses;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByEaterId;

public class GetAllOrderByEaterIdHandler : IRequestHandler<GetAllOrderByEaterIdQuery, GetAllOrderByUserIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetAllOrderByEaterIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllOrderByUserIdResponse> Handle(GetAllOrderByEaterIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllOrderByUserIdResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var eaterId = request.EaterId;
            var orders = _unitOfRepository.Order.Where(o => o.EaterId == eaterId).AsNoTracking().ToList();
            response.StatusCode = (int)ResponseStatusCode.OK;
            response.StatusText = "Get all orders by eaterid successfully";
            response.Data = orders.Select(o => o.AsDto()).ToList();
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