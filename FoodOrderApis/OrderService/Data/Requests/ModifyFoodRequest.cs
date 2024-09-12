namespace OrderService.Data.Requests;

public class ModifyFoodRequest
{
    public int FoodId { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public string ImageUrl { get; set; }
}
