using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientGrantTypes
{
    [Key]
    public int Id { get; set; }
    public string ClientId { get; set; }
    public string GrantType { get; set; }
}
