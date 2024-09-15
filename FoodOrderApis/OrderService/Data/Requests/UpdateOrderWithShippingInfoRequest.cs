namespace OrderService.Data.Requests;

public class UpdateOrderWithShippingInfoRequest
{
    public int OrderId { get; set; }
    public decimal ShippingFee { get; set; }
    public string ShippingAddress { get; set; }
    public string ShippingPhoneNumber { get; set; }
}