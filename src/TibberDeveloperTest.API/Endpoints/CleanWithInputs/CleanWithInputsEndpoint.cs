using FluentValidation;
using TibberDeveloperTest.Application.Abstractions.Messaging;
using TibberDeveloperTest.Application.Commands.CleanWithInputs;
using TibberDeveloperTest.Domain.Enums;

namespace TibberDeveloperTest.API.Endpoints.CleanWithInputs;

public class CleanWithInputsEndpoint : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("/tibber-developer-test/enter-path", async (
            CleanWithInputsRequest request, 
            ICommandHandler<CleanWithInputsCommand, CleanWithInputsDto> handler, 
            IValidator<CleanWithInputsRequest> validator,
            CancellationToken cancellationToken) =>
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);

            if (!validationResult.IsValid)
            {
                return Results.BadRequest(validationResult.Errors);
            }
            
            var result = await handler.Handle(new CleanWithInputsCommand(new StartDto(request.Start.X, request.Start.Y),
                request.Commands.Select(x => new CommandDto((Direction)Enum.Parse(typeof(Direction), x.Direction, ignoreCase: true), x.Steps)).ToList()), cancellationToken);
            
            return Results.Ok(result); // would be good to have a mapping layer and return "ClearWithInputsResponse", since we might not want to return the whole Dto to the user f.e. the Id property
        });
    }
}