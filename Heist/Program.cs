using Heist.Core.Interfaces.Repository;
using Heist.Core.Interfaces.Services;
using Heist.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Heist.Core.Interfaces;
using Heist.Infrastructure;





var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Register controllers
builder.Services.AddControllers();


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

// Register services and repositories for Dependency Injection
builder.Services.AddScoped<IMemberService, MemberService>();
builder.Services.AddScoped<IMemberRepository, MemberRepository>();


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
