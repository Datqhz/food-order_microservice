using Microsoft.AspNetCore.Http;

namespace FoodOrderApis.Common.HttpContextCustom;

public interface ICustomHttpContextAccessor
{
    string GetCurrentUserId();
    string GetClientId();
}
public class CustomHttpContextAccessor : ICustomHttpContextAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CustomHttpContextAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    public string GetCurrentUserId() => _httpContextAccessor.HttpContext.User.FindFirst(Constants.Constants.CustomClaimTypes.UserId)?.Value;
    public string GetClientId() => _httpContextAccessor.HttpContext.User.FindFirst(Constants.Constants.CustomClaimTypes.ClientId)?.Value;
}
