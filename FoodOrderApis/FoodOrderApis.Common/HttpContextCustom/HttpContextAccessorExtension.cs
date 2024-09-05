using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace FoodOrderApis.Common.HttpContextCustom;

public static class HttpContextAccessorExtension
{
    public static IServiceCollection AddCustomHttpContextAccessor(this IServiceCollection services)
    {
        services.AddHttpClient();
        services.AddHttpContextAccessor();
        services.TryAddSingleton<ICustomHttpContextAccessor, CustomHttpContextAccessor>();
        return services;
    }
}
