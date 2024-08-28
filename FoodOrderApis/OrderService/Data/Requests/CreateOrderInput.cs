namespace OrderService.Data.Requests;

public class CreateOrderInput
{
    public int EaterId { get; set; }
    public int MerchantId { get; set; }
}
