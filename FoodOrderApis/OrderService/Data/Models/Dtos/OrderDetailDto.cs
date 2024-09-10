namespace OrderService.Data.Models.Dtos;

public class OrderDetailDto
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public FoodDto Food { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}
