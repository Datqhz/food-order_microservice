using OrderService.Enums;

namespace OrderService.Data.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderedDate { get; set; }
    public int OrderStatus { get; set; }
    public string EaterId { get; set; }
    public string MerchantId { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
