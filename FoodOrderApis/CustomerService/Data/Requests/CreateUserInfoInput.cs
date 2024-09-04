namespace CustomerService.Data.Requests;

public class CreateUserInfoInput
{
    public string UserId { get; set; }
    public string ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string UserName { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
}