using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientScope
{
    [Key]
    public int Id { get; set; }
    public string Scope { get; set; }
    public int ClientId { get; set; }
}
