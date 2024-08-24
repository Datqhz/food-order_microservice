using Microsoft.IdentityModel.Tokens;

namespace CustomerService.Extensions;

public class ServiceExtensions
{
    // public void ConfigureDbContext(IServiceCollection services)
    // {
    //     services.AddDbContext<>()
    // }
    public void AddAuthentication(IServiceCollection services)
    {
        services
            .AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.Authority = "http://localhost:5092";
                options.TokenValidationParameters = new TokenValidationParameters
                {

                    ValidateIssuer = false,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true
                };
            });
    }
}