namespace CustomerService.Data.Models;

public class UserInfo
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public int ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
}