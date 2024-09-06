namespace FoodOrderApis.Common.MassTransit.Contracts;

public record CreateFood
{
    public int Id { get; init; }
    public string Name { get; init; }
    public string Describe { get; init; }
    public string ImageUrl { get; init; }
}
