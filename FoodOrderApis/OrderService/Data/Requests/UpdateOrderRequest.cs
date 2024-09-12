namespace OrderService.Data.Requests;

public class UpdateOrderRequest
{
    public int OrderId { get; set; }
    public bool? Cancellation { get; set; }
}
