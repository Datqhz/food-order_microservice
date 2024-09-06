﻿using FoodOrderApis.Common.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderService.Data.Models.Dtos;
using OrderService.Data.Responses;
using OrderService.Extensions;
using OrderService.Repositories;
using OrderService.Repositories.Interfaces;

namespace OrderService.Features.Queries.OrderDetailQueries.GetAllOrderDetailByOrderId;

public class GetAllOrderDetailByOrderIdHandler : IRequestHandler<GetAllOrderDetailByOrderIdQuery, GetAllOrderDetailByOrderIdResponse>
{
    private readonly IUnitOfRepository _unitOfRepository;
    private readonly ILogger<GetAllOrderDetailByOrderIdHandler> _logger;

    public GetAllOrderDetailByOrderIdHandler(IUnitOfRepository unitOfRepository, ILogger<GetAllOrderDetailByOrderIdHandler> logger)
    {
        _unitOfRepository = unitOfRepository;
        _logger = logger;
    }
    public async Task<GetAllOrderDetailByOrderIdResponse> Handle(GetAllOrderDetailByOrderIdQuery request, CancellationToken cancellationToken)
    {
        var functionName = nameof(GetAllOrderDetailByOrderIdHandler);
        var response = new GetAllOrderDetailByOrderIdResponse(){StatusCode = (int)ResponseStatusCode.NoContent};
        var orderId = request.OrderId;
        try
        {
            _logger.LogInformation($"{functionName} - Start");
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
            _logger.LogInformation($"{functionName} - End");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError($"{functionName} => Has error : Message = {ex.Message}");
            response.StatusCode = (int)ResponseStatusCode.InternalServerError;
            response.StatusText = "Internal Server Error";
            response.ErrorMessage = ex.Message;
            return response;
        }
    }
}
