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
    [property:JsonPropertyName("commands")] List<CommandDto> Commands) : ICommand<CleanWithInputsDto>;

public sealed class CleanWithInputsCommandHandler(IExecutionsRepository executionsRepository) : ICommandHandler<CleanWithInputsCommand, CleanWithInputsDto>
{
    public async Task<CleanWithInputsDto> Handle(CleanWithInputsCommand command, CancellationToken cancellationToken)
    {
        var commandsCount = command.Commands.Count;
        var cleaningRobot = new CleaningRobot("tibberock S5 MAX");
        
        cleaningRobot.Start();
        ///////
        var stopwatch = Stopwatch.StartNew();
        cleaningRobot.SetInitialPosition(command.Start.X, command.Start.Y);
        
        for (var i = 0; i < commandsCount; i++)
        {
            var moveCommand = command.Commands[i];
            cleaningRobot.Move(moveCommand.Direction, moveCommand.Steps);
        }
        stopwatch.Stop();
        ///////
        cleaningRobot.Stop();

        var execution = await executionsRepository.AddAsync(new Execution
        {
            Commands = commandsCount,
            Result = cleaningRobot.UniquePositions.Count,
            DurationS = (decimal)stopwatch.Elapsed.TotalSeconds
        }, cancellationToken);
        
        return new CleanWithInputsDto(execution.Id, execution.Timestamp, commandsCount, execution.Result, execution.DurationS);
    }
}