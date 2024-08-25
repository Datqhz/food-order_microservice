using ApiGateway.Extensions;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

var builder = WebApplication.CreateBuilder(args);

ServiceExtensions serviceExtensions = new ServiceExtensions(builder.Configuration);
serviceExtensions.ConfigureAuthentication(builder.Services);
builder.Configuration.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);
builder.Configuration.AddOcelotWithSwaggerSupport(options =>
{
    options.Folder = "Routes";
});
builder.Services.AddOcelot(builder.Configuration);
serviceExtensions.ConfigureSwagger(builder.Services);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerForOcelotUI(opt =>
    {
        opt.PathToSwaggerGenerator = "/swagger/docs";
    });
}
// app.UseHttpsRedirection();
app.UseAuthentication();

await app.UseOcelot();
app.Run();
