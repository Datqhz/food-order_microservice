namespace CustomerService.Data.Requests;

public class UpdateUserInfoInput
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
}
