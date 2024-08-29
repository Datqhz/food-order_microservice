namespace OrderService.Data.Models.Dtos;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public FoodDto Food { get; set; }
}
