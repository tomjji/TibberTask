using System.Text.Json.Serialization;
using FluentValidation;

namespace TibberDeveloperTest.API.Endpoints.CleanWithInputs;

public record Command(
    [property:JsonPropertyName("direction")] string Direction,
    [property:JsonPropertyName("steps")] int Steps);

public record Start(
    [property:JsonPropertyName("x")] int X,
    [property:JsonPropertyName("y")] int Y);

public record CleanWithInputsRequest(
    [property: JsonPropertyName("start")] Start Start,
    [property: JsonPropertyName("commands")] List<Command> Commands);
    
public class CleanWithInputsValidator : AbstractValidator<CleanWithInputsRequest>
{
    // skipping office bounds validation here and in the domain, since "The robot will never be sent outside the bounds of the office."
    public CleanWithInputsValidator()
    {
        // starting coordinates
        RuleFor(request => request.Start.X)
            .InclusiveBetween(-100000, 100000)
            .WithMessage("The X coordinate must be between -100,000 and 100,000.");

        RuleFor(request => request.Start.Y)
            .InclusiveBetween(-100000, 100000)
            .WithMessage("The Y coordinate must be between -100,000 and 100,000.");

        // commands
        RuleFor(request => request.Commands)
            .Must(commands => commands.Count >= 0 && commands.Count <= 10000)
            .WithMessage("The number of commands must be between 0 and 10,000.");
        RuleForEach(request => request.Commands)
            .ChildRules(commands =>
            {
                commands.RuleFor(command => command.Direction)
                    .Must(direction => new[] { "north", "east", "south", "west" }.Contains(direction.ToLower()))
                    .WithMessage("Invalid direction. Allowed values are 'north', 'east', 'south', and 'west'.");
                
                commands.RuleFor(command => command.Steps)
                    .InclusiveBetween(1, 99999)
                    .WithMessage("Steps must be between 1 and 99,999.");
            });
    }
}