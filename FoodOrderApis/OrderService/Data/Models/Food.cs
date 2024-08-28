namespace OrderService.Data.Models;

public class Food
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public string ImageUrl { get; set; }
    public List<OrderDetail> OrderDetails { get; set; }
}
