using System.ComponentModel.DataAnnotations;

namespace AuthServer.Data.Models;

public class ApiResources
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string DisplayName { get; set; }
}
