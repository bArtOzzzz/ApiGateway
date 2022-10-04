using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Service.Extention;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("ocelot.json")
    .AddEnvironmentVariables();

builder.Services.AddOcelot();

builder.Services.AddCustomJwtAuthentication();

// Enable CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins", config =>
    {
        config.SetIsOriginAllowedToAllowWildcardSubdomains()
              .WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .Build();
    });
});

var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();

app.UseAuthentication();

app.UseOcelot();

app.Run();