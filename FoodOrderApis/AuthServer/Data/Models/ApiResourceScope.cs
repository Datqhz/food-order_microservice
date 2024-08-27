using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ApiResourceScope
{
    [Key]
    public int Id { get; set; }
    public string Scope { get; set; }
    public int ApiResourceId { get; set; }
}
