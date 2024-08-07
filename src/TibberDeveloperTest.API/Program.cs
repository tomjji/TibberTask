using System.Reflection;
using Microsoft.EntityFrameworkCore;
using TibberDeveloperTest.API.Extensions;
using TibberDeveloperTest.Infrastructure.Postgres.Contexts;

var builder = WebApplication.CreateBuilder(args);
builder.Services.RegisterServices();
builder.Services.AddEndpoints(Assembly.GetExecutingAssembly());
var app = builder.Build();
app.MapHealthChecks("/health");
app.MapEndpoints();
app.ConfigureSwagger();

Console.WriteLine("Applying Migrations");
// Run migrations on API startup for simplicity
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var dbContext = services.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate();
}
Console.WriteLine("Migrations applied");

app.Run();