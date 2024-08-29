using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models;
using OrderService.Data.Models.Dtos;
using OrderService.Data.Responses;
using OrderService.Extensions;
using OrderService.Repositories;

namespace OrderService.Features.Queries.OrderQueries.GetAllOrderByUserId;

public class GetAllOrderByUserIdHandler : IRequestHandler<GetAllOrderByUserIdQuery, GetAllOrderByUserIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;

    public GetAllOrderByUserIdHandler(IUnitOfRepository unitOfRepository)
    {
        _unitOfRepository = unitOfRepository;
    }
    public async Task<GetAllOrderByUserIdResponse> Handle(GetAllOrderByUserIdQuery request, CancellationToken cancellationToken)
    {
        var response = new GetAllOrderByUserIdResponse() {StatusCode = (int)ResponseStatusCode.BadRequest};
        try
        {
            var validator = new GetAllOrderByUserIdValidator();
            var validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                response.StatusText = validationResult.ToString("~");
                return response;
            }
            var userId = request.EaterId != null ? request.EaterId : request.MerchantId;
            List<Order> orders;
            if (request.EaterId != null)
            {
                orders = _unitOfRepository.Order.Where(o => o.EaterId == userId).AsNoTracking().ToList();
            }
            else
            {
                orders = _unitOfRepository.Order.Where(o => o.MerchantId == userId).AsNoTracking().ToList();
            }
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