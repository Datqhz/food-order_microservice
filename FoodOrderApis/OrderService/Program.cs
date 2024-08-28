using OrderService.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
ServiceExtensions serviceExtensions = new ServiceExtensions(builder.Configuration);
serviceExtensions.AddAuthentication(builder.Services);
serviceExtensions.ConfigureDbContext(builder.Services);
//serviceExtensions.ConfigureDependencyInjection(builder.Services);
serviceExtensions.AddMediatorPattern(builder.Services);
serviceExtensions.AddCors(builder.Services);
serviceExtensions.AddMassTransitRabbitMq(builder.Services);
builder.Services.AddControllers();
serviceExtensions.ConfigureSwagger(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.Run();
