using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TibberDeveloperTest.Application.Abstractions.Messaging;
using TibberDeveloperTest.Application.Commands.CleanWithInputs;
using TibberDeveloperTest.Application.Interfaces;
using TibberDeveloperTest.Infrastructure.Postgres.Contexts;
using TibberDeveloperTest.Infrastructure.Postgres.Repositories;

namespace TibberDeveloperTest.API.Extensions;

public static class DependencyInjectionSetup
{
    public static void RegisterServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddHealthChecks();
        services.AddSwaggerGen();
        services.AddValidatorsFromAssemblyContaining<Program>();
        
        // it is possible to register all handlers automatically with assembly scanning using Scrutor or an equivalent. For the sake of simplicity though...
        services.AddScoped<ICommandHandler<CleanWithInputsCommand, CleanWithInputsDto>, CleanWithInputsCommandHandler>();
        
        var connectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        services.AddDbContextFactory<ApplicationDbContext>(options =>
        {
            options.LogTo(Console.WriteLine, LogLevel.Warning);
            options.UseNpgsql(connectionString);
        });
        services.AddScoped<IExecutionsRepository, ExecutionsRepository>();
    }
}