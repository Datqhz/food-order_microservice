namespace AuthServer.Data.Dtos.Inputs;

public class GetTokenRequestInput
{
    public string username { get; set; } = string.Empty;
    public string password { get; set; } = string.Empty;
    public string client_id { get; set; } = string.Empty;
    public string client_secret { get; set; } = string.Empty;
    public string scope { get; set; } = string.Empty;
}