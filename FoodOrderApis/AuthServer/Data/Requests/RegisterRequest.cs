namespace AuthServer.Data.Requests;

public class RegisterRequest
{
    public string Displayname { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public string ClientId { get; set; }
    public string Role { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar {get; set;} = "https://static.vecteezy.com/system/resources/previews/009/292/244/original/default-avatar-icon-of-social-media-user-vector.jpg";
}
