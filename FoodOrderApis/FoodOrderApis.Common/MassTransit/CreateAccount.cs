namespace FoodOrderApis.Common.MassTransit.Consumers;

public record CreateAccount
{
    public int AccountId { get; set; }
    public string Username { get; set; }
}
