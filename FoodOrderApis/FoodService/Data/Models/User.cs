namespace FoodService.Data.Models;

public class User
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    public List<Food> Foods { get; set; }
}
