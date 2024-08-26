using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientGrantType
{
    [Key]
    public int Id { get; set; }
    public int ClientId { get; set; }
    public string GrantType { get; set; }
}
