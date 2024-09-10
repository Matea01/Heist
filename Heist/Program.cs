using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;
using Heist.Infrastructure.Database;
using Heist.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Configure logging
builder.Logging.ClearProviders(); // Remove default providers (like Console)
builder.Logging.AddConsole(); // Add Console logging
builder.Logging.AddDebug(); // Add Debug logging
// Add other logging providers if needed

// Add services to the container.


// Register controllers and configure JSON options for enum string serialization
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configure enums to be serialized as strings instead of numeric values
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true;
    });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Heist API",
        Version = "v1"
    });
});

// Register the DbContext for EF Core,
builder.Services.AddDbContext<HeistDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    options.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 2)), options =>
    {
        options.MigrationsAssembly("Heist.Infrastructure");
    });
});

// Add services
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IHeistService, HeistService>();
// Add repositories
builder.Services.AddScoped<IMemberRepository, MemberRepository>();
builder.Services.AddScoped<IHeistRepository, HeistRepository>();
builder.Services.AddScoped<ISkillRepository, SkillRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    // Enable Swagger in development mode for testing APIs
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Heist API v1");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
