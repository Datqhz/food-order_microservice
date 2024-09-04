namespace AuthServer.Data.Requests;

public class RegisterInput
{
    public string Displayname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string PhoneNumber { get; set; }
}
