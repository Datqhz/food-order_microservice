using Microsoft.AspNetCore.Identity;

namespace AuthServer.Data.Models;

public class User : IdentityUser
{
    public override string Id { get; set; }
    public string Displayname { get; set; } = string.Empty;
    public int ClientId { get; set; }
    public DateTime CreatedDate { get; set; }
    public bool IsActive { get; set; } =  true;
}
