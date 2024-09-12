namespace OrderService.Data.Requests;

public class ModifyUserRequest
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
}
