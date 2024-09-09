using AuthServer.Extensions;
using AuthServer.Middlewares;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Extensions;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions  serviceExtensions = new ServiceExtensions(builder.Configuration);
serviceExtensions.ConfigureDbContext(builder.Services);
serviceExtensions.ConfigureDependencyInjection(builder.Services);
serviceExtensions.ConfigureIdentityServer(builder.Services);
serviceExtensions.AddValidators(builder.Services);
serviceExtensions.ConfigureMediator(builder.Services);
serviceExtensions.AddCors(builder.Services);
builder.Services.AddMassTransitRegistration(null);
builder.Services.AddControllers();
builder.Services.AddCustomHttpContextAccessor();
serviceExtensions.ConfigureSwagger(builder.Services);
var app = builder.Build();
// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseIdentityServer();
app.UseAuthorization();
app.MapControllers();
app.Run();
