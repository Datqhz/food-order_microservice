using CustomerService.Extensions;
using CustomerService.Middlewares;
using CustomerService.StartupRegistrations;
using FoodOrderApis.Common.HttpContextCustom;
using FoodOrderApis.Common.MassTransit.Extensions;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions serviceExtensions = new ServiceExtensions(builder.Configuration);
serviceExtensions.AddAuthentication(builder.Services);
serviceExtensions.AddAuthorizationSettings(builder.Services);
serviceExtensions.ConfigureDbContext(builder.Services);
serviceExtensions.ConfigureDependencyInjection(builder.Services);
serviceExtensions.AddValidators(builder.Services);
serviceExtensions.AddMediatorPattern(builder.Services);
serviceExtensions.AddCors(builder.Services);
builder.Services.AddCustomMassTransitRegistration();
builder.Services.AddControllers();
builder.Services.AddCustomHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
serviceExtensions.ConfigureSwagger(builder.Services);

var app = builder.Build();
//serviceExtensions.InitializeDatabase(app);
// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// app.UseHttpsRedirection();

app.Run();
