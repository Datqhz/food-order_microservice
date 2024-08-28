using OrderService.Enums;

namespace OrderService.Data.Models;

public class Order
{
    public int Id { get; set; }
    public DateTime OrderedDate { get; set; }
    public int OrderStatus { get; set; }
    public int EaterId { get; set; }
    public int MerchantId { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
