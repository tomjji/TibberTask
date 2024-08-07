namespace TibberDeveloperTest.API.Extensions;

public static class SwaggerConfigurationSetup
{
    public static void ConfigureSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}