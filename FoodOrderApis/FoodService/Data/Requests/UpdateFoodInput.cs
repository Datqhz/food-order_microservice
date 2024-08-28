namespace FoodService.Data.Requests;

public class UpdateFoodInput
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string ImageUrl { get; set; }
    public string Describe { get; set; }
    public decimal Price { get; set; }
}
