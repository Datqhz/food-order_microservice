namespace AuthServer.Data.Requests;

public class UpdateUserRequest
{
    public string UserId { get; set; }
    public string NewPassword { get; set; }
    public string OldPassword {get; set;}
}
