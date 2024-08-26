using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ClientSecrets
{
    [Key]
    public int Id { get; set; }
    public string SecretName { get; set; }
    public string ClientId { get; set; }
}
