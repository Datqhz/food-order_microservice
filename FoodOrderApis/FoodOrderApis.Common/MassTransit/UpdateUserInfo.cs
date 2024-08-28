namespace FoodOrderApis.Common.MassTransit;

public class UpdateUserInfo
{
    public string UserId { get; set; }
    public bool IsActive { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
}
