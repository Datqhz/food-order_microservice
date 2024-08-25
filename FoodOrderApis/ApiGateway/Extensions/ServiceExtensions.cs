using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;

namespace ApiGateway.Extensions;

public class ServiceExtensions
{
    private readonly IConfiguration _configuration;

    public ServiceExtensions(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public void ConfigureSwagger(IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerForOcelot(_configuration);
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "API gateway Document",
                Version = "v1",
                Description = " is the api detailed description for the project",
            });
        });
    }
    public void ConfigureAuthentication(IServiceCollection services)
    {
        services.AddAuthentication("Bearer")
            .AddJwtBearer("Bearer", options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = CreateRsaKey()
                };
            });
    }
    private RsaSecurityKey CreateRsaKey()
    {
        string publicKey =
            "k_9bwv79mE3vz7qU82LziCxwRjLdjAgby84sk10uIYdEwiSbmHDTFoHt8DcwngE9XF2eQCiFlKL5hVL1weF7lBZ4sJTcHf57ei6clEhNGzTtmakEfU2cGF4Wk9EgLZizfJsIrr7aBL5DgOPKd-b9xzYQxtTCWln8JcRZtR_TJtHp79t3yGKabzKuA8oVLcGHc9Y2OxIppWeZjD6S0SuliFdLfDT0jjFvhYEkY664MEdYLgx9HKDqI1VFvftFJ3-UkxOEKVwiEp2FDyY1IJ8PnPkD9jUmoWPD5Xbd8fkHEKxjr52gCCXHIKh4OkHXtbNDJfSYDh2juqoI3xChyV4RLQ";
        string exponent = "AQAB";
        var publicKeyAsBytes = Base64UrlEncoder.DecodeBytes(publicKey);
        var exponentBytes = Base64UrlEncoder.DecodeBytes(exponent);
        var rsaParameter = new RSAParameters
        {
            Modulus = publicKeyAsBytes,
            Exponent = exponentBytes
        };
        var rsaKey = RSA.Create();
        rsaKey.ImportParameters(rsaParameter);
        return new RsaSecurityKey(rsaKey);
    }

}