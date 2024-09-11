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
            Eater  = order.Eater?.AsDto(),
            Merchant = order.Merchant?.AsDto(),
        };
    }

    public static OrderDetailDto AsDto(this OrderDetail orderDetail)
    {
        return new OrderDetailDto
        {
            Id = orderDetail.Id,
            OrderId = orderDetail.OrderId,
            Food = orderDetail.Food.AsDto(),
        };
    }
    public static FoodDto AsDto(this Food food)
    {
        return new FoodDto()
        {
            Id = food.Id,
            Name = food.Name,
            ImageUrl = food.ImageUrl,
            Describe = food.Describe
        };
    }

    public static UserDto AsDto(this User user)
    {
        return new UserDto
        {
            Id = user.UserId,
            DisplayName = user.DisplayName,
            PhoneNumber = user.PhoneNumber,
        };
    }
}
