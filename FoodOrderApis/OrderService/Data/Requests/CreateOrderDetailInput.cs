namespace OrderService.Data.Requests;

public class CreateOrderDetailInput
{
    public int OrderId { get; set; }
    public int FoodId { get; set; }
    public int Quantity { get; set; }
    public int Price { get; set; }
}
