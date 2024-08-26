using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientSecret
{
    [Key]
    public int Id { get; set; }
    public string SecretName { get; set; }
    public int ClientId { get; set; }
}
