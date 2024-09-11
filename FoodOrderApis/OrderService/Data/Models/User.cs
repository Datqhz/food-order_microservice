using Newtonsoft.Json;

namespace OrderService.Data.Models;

public class User
{
    public string UserId { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    [JsonIgnore]
    public List<Order>? OrderCreateds { get; set; }
    [JsonIgnore]
    public List<Order>? OrderReceiveds { get; set; }
}
