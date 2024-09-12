namespace OrderService.Data.Requests;

public class CreateOrderRequest
{
    public string EaterId { get; set; }
    public string MerchantId { get; set; }
}
