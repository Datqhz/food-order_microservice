namespace FoodService.Data.Requests;

public class CreateFoodInput
{
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Describe { get; set; }
    public decimal Price { get; set; }
    public string UserId { get; set; }
}
