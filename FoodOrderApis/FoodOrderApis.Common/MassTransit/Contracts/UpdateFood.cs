namespace FoodOrderApis.Common.MassTransit.Contracts;

public class UpdateFood
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Describe { get; set; }
    public string ImageUrl { get; set; }
}
