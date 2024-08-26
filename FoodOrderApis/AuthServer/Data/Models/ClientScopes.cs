using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientScopes
{
    [Key]
    public int Id { get; set; }
    public string Scope { get; set; }
    public string ClientId { get; set; }
}
