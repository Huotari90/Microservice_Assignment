//ElectricityPriceMonitorMS
//Program.cs 
using Microsoft.EntityFrameworkCore;
using ElectricityPriceMonitorMS.Services;
using ElectricityPriceMonitorMS.Data;

var builder = WebApplication.CreateBuilder(args);

// Add CORS policy for development
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Add services to the container.
builder.Services.AddHttpClient<ElectricityPriceService>();
builder.Services.AddHostedService<ElectricityPriceBackgroundService>();
builder.Services.AddDbContext<ElectricityContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddEndpointsApiExplorer(); // This is required for Swagger
builder.Services.AddSwaggerGen(); // This adds Swagger generation services

var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Enable middleware to serve swagger-ui
    app.UseCors("AllowAll"); // Apply CORS policy
}

app.MapGet("/latest-prices", async (ElectricityPriceService service) =>
{
    try
    {
        var prices = await service.GetLatestPricesAsync();
        return Results.Ok(prices);
    }
    catch (HttpRequestException ex)
    {
        // Log the exception details
        return Results.Problem(detail: ex.Message, statusCode: 503);
    }
});

app.Run();
