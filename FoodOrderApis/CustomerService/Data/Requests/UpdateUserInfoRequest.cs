﻿namespace CustomerService.Data.Requests;

public class UpdateUserInfoRequest
{
    public string Id { get; set; }
    public string DisplayName { get; set; }
    public string PhoneNumber { get; set; }
    public string Avatar { get; set; }
}
