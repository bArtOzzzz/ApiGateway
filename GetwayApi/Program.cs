﻿using GetwayApi.Extention;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add Configuration for Ocelot
builder.Configuration.SetBasePath(builder.Environment.ContentRootPath)
                     .AddJsonFile("ocelot.json")
                     .AddEnvironmentVariables();

// Add Ocelot
builder.Services.AddOcelot();

// Add Extension
builder.Services.DecorateClaimAuthoriser();

// Add JWT Token validation
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.RequireHttpsMetadata = false;
    option.SaveToken = true;
    option.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("JsonWebApiTokenWithSwaggerAuthorizationAuthenticationAspNetCore"))
    };
});

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