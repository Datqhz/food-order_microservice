namespace FoodOrderApis.Common.MassTransit.Consumers;

public record CreateAccount
{
    public string AccountId { get; set; }
    public string Username { get; set; }
    public DateTime CreatedDate { get; set; }
    public string PhoneNumber { get; set; }
}
