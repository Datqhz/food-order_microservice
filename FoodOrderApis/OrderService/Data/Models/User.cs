namespace OrderService.Data.Models;

public class User
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Order>? OrderCreateds { get; set; }
    public List<Order>? OrderReceiveds { get; set; }
}
