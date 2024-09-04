using CustomerService.Extensions;
using FoodOrderApis.Common.MassTransit.Extensions;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions serviceExtensions = new ServiceExtensions(builder.Configuration);
serviceExtensions.AddAuthentication(builder.Services);
serviceExtensions.AddAuthorizationSettings(builder.Services);
serviceExtensions.ConfigureDbContext(builder.Services);
serviceExtensions.ConfigureDependencyInjection(builder.Services);
serviceExtensions.AddMediatorPattern(builder.Services);
serviceExtensions.AddCors(builder.Services);
//serviceExtensions.AddMassTransitRabbitMq(builder.Services);
builder.Services.AddMassTransitRegistration("customer");
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
serviceExtensions.ConfigureSwagger(builder.Services);

var app = builder.Build();
//serviceExtensions.InitializeDatabase(app);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAllOrigins");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
// app.UseHttpsRedirection();

app.Run();
