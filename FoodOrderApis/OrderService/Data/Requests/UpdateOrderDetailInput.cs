namespace OrderService.Data.Requests;

public class UpdateOrderDetailInput
{
    public int OrderDetailId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public int Feature { get; set; }
}