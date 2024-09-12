namespace OrderService.Data.Requests;

public class ModifyOrderDetailRequest
{
    public int? OrderDetailId { get; set; }
    public int? OrderId { get; set; }
    public int? FoodId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int Feature { get; set; }
}
