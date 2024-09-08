namespace OrderService.Data.Requests;

public class CreateOrderDetailInput
{
    public int OrderId { get; set; }
    public int FoodId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
