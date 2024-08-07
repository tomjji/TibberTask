using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using TibberDeveloperTest.API.Endpoints;

namespace TibberDeveloperTest.API.Extensions;

public static class EndpointExtensions
{
    public static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var endpointServiceDescriptors = assembly
            .DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();
        services.TryAddEnumerable(endpointServiceDescriptors);
    }

    public static void MapEndpoints(this WebApplication app)
    {
        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>();
        foreach (var endpoint in endpoints)
        {
            endpoint.MapEndpoint(app);
        }
    }
}