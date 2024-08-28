namespace OrderService.Data.Requests;

public class UpdateOrderInput
{
    public int OrderId { get; set; }
    public int OrderStatus { get; set; }
}
