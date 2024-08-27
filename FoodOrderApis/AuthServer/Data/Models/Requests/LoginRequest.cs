﻿namespace AuthServer.Data.Models.Requests;

public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Scope { get; set; } = string.Empty;
}
