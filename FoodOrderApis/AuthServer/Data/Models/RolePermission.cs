namespace AuthServer.Data.Models;

public class RolePermission
{
    public int Id { get; set; }
    public string Permission { get; set; }
    public string Role { get; set; }
}
