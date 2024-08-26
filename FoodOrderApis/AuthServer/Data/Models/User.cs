namespace AuthServer.Data.Models;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Displayname { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } =  true;
}
