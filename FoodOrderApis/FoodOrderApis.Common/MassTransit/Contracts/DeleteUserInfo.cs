namespace FoodOrderApis.Common.MassTransit.Contracts;

public record DeleteUserInfo
{
    public string UserId { get; init; }
}
