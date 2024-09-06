namespace FoodOrderApis.Common.MassTransit.Contracts;

public record UpdateUserInfo
{
    public string UserId { get; init; }
    public bool IsActive { get; init; }
    public string DisplayName { get; init; }
    public string PhoneNumber { get; init; }
}
