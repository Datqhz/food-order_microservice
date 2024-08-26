using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class Client
{
    [Key]
    public int Id { get; set; }
    public string ClientId { get; set; }
    public string ClientName { get; set; }
}
