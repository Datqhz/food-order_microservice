namespace FoodOrderApis.Common.MassTransit.Contracts;

public record CreateUserInfo
{
    public string UserId { get; init; }
    public string Role { get; init; }
    public DateTime CreatedDate { get; init; }
    public bool IsActive { get; init; }
    public string UserName { get; init; }
    public string DisplayName { get; init; }
    public string PhoneNumber { get; init; }
}
