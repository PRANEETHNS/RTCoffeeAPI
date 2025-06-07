using RTCoffeeAPI.Services.Interfaces;
using RTCoffeeAPI.Services;
using Microsoft.AspNetCore.Mvc;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 1. Register versioning services:
builder.Services.AddApiVersioning(options =>
{
    // Report API versions in response headers: "api-supported-versions" etc.
    options.ReportApiVersions = true;

    // Default to version 1.0 if no version is specified
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);

    // Use URL segment versioning (e.g. /api/v1/...)
    options.ApiVersionReader = new Microsoft.AspNetCore.Mvc.Versioning.UrlSegmentApiVersionReader();
});

//Service registered limited to the run
builder.Services.AddHttpClient<IWeatherService, WeatherService>();
builder.Services.AddScoped<ICoffeeService, CoffeeService>();

builder.Logging.ClearProviders();
builder.Host.UseNLog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

