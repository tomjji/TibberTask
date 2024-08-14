using System.Diagnostics;
using System.Text.Json.Serialization;
using TibberDeveloperTest.Application.Abstractions.Messaging;
using TibberDeveloperTest.Application.Interfaces;
using TibberDeveloperTest.Domain;
using TibberDeveloperTest.Domain.Entities;
using TibberDeveloperTest.Domain.Enums;

namespace TibberDeveloperTest.Application.Commands.CleanWithInputs;

public record CommandDto(Direction Direction, int Steps);
public record StartDto(int X, int Y);
public record CleanWithInputsCommand(
    [property:JsonPropertyName("start")] StartDto Start, 
    [property:JsonPropertyName("commands")] CommandDto[] Commands) : ICommand<CleanWithInputsDto>;

public sealed class CleanWithInputsCommandHandler(IExecutionsRepository executionsRepository) : ICommandHandler<CleanWithInputsCommand, CleanWithInputsDto>
{
    private const int GridSize = 200000;
    
    public async Task<CleanWithInputsDto> Handle(CleanWithInputsCommand command, CancellationToken cancellationToken)
    {
        var commandsCount = command.Commands.Length;
        var cleaningRobot = new CleaningRobot("tibberock S5 MAX", GridSize);
        
        cleaningRobot.Start();
        ///////
        var stopwatch = Stopwatch.StartNew();
        cleaningRobot.SetInitialPosition(command.Start.X, command.Start.Y);
        var startingPosition = new Point(command.Start.X, command.Start.Y);
        for (var i = 0; i < commandsCount; i++)
        {
            var currentCommand = command.Commands[i];
            cleaningRobot.Move(startingPosition.X, startingPosition.Y, currentCommand.Direction, currentCommand.Steps);
            UpdateStartingPosition(startingPosition, currentCommand.Direction, currentCommand.Steps);
        }
        stopwatch.Stop();
        ///////
        var execution = await executionsRepository.AddAsync(new Execution
        {
            Commands = commandsCount,
            Result = cleaningRobot.UniquePoints,
            DurationS = (decimal)stopwatch.Elapsed.TotalSeconds
        }, cancellationToken);
        
        cleaningRobot.Stop();
        
        return new CleanWithInputsDto(execution.Id, execution.Timestamp, commandsCount, execution.Result, execution.DurationS);
    }

    private static void UpdateStartingPosition(Point point, Direction direction, int steps)
    {
        switch (direction)
        {
            case Direction.North: point.Y += steps; break;
            case Direction.South: point.Y -= steps; break;
            case Direction.East: point.X += steps; break;
            case Direction.West: point.X -= steps; break;
            default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}