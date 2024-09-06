using System.Security.Claims;
using AuthServer.Data.Models;
using FoodOrderApis.Common.Constants;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;

namespace AuthServer.Core;

public class ProfileService : IProfileService
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<ProfileService> _logger;

    public ProfileService(UserManager<User> userManager, ILogger<ProfileService> logger)
    {
        _userManager = userManager;
        _logger = logger;
    }
    public async Task GetProfileDataAsync(ProfileDataRequestContext context)
    {
        try
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            var roles = await _userManager.GetRolesAsync(user);
            var claims = new List<Claim>()
            {
                new Claim(Constants.CustomClaimTypes.UserId, sub),
                new Claim(Constants.CustomClaimTypes.Role, roles[0]),
                new Claim(Constants.CustomClaimTypes.UserName, user.UserName),
            };
            context.IssuedClaims.AddRange(claims);
        }
        catch (Exception ex)
        {
            _logger.LogError($"{nameof(ProfileService)} Has error: Message = {ex.Message}");
        }
    }

    public async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject.GetSubjectId();
        var user = await _userManager.FindByIdAsync(sub);
        context.IsActive = user != null;
    }
}
