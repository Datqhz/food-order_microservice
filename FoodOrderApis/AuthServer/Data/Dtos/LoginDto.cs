namespace AuthServer.Data.Dtos;

public class LoginDto
{
    public string AccessToken { get; set; }
    public string Scope { get; set; }
    public int Expired  { get; set; }
}