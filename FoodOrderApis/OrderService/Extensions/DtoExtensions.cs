using OrderService.Data.Models;
using OrderService.Data.Models.Dtos;

namespace OrderService.Extensions;

public static class DtoExtensions
{
    public static OrderDto AsDto(this Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            EaterId = order.EaterId,
            MerchantId = order.MerchantId,
            OrderedDate = order.OrderedDate,
            OrderStatus = order.OrderStatus,
        };
    }
}