namespace OrderService.Data.Models.Dtos;

public class OrderDto
{
    public int Id { get; set; }
    public DateTime OrderedDate { get; set; }
    public int OrderStatus { get; set; }
    public string? EaterId { get; set; }
    public string? MerchantId { get; set; }
    public UserDto? Eater { get; set; }
    public UserDto? Merchant { get; set; }
}