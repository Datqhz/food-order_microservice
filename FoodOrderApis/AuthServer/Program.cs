using AuthServer.Extensions;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions  serviceExtensions = new ServiceExtensions();
serviceExtensions.AddAuthenticationSettings(builder.Services);
serviceExtensions.ConfigureMediator(builder.Services);
serviceExtensions.AddCors(builder.Services);
serviceExtensions.AddMassTransitWithRabbitMq(builder.Services);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAnyOrigin");
app.UseIdentityServer();
app.MapControllers();
app.Run();
