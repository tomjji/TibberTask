using FluentAssertions;
using NSubstitute;
using TibberDeveloperTest.Application.Commands.CleanWithInputs;
using TibberDeveloperTest.Application.Interfaces;
using TibberDeveloperTest.Domain.Entities;
using TibberDeveloperTest.Domain.Enums;

namespace TibberDeveloperTest.Application.Tests;

public class CleanWithInputsCommandHandlerTests
{
    [Fact]
    public async Task CleanWithInputsCommandHandler_ShouldReturnExpectedResults()
    {
        // Arrange
        var executionsRepositoryMock = Substitute.For<IExecutionsRepository>();

        var executionId = 777;
        var commands = new CommandDto[]
        {
            new(Direction.East, 2),
            new(Direction.North, 1),
            new(Direction.South, 3), // skip 1
            new(Direction.West, 4)
        };
        
        var command = new CleanWithInputsCommand(
            new StartDto(10, 10),
            commands
        );

        executionsRepositoryMock.
            AddAsync(Arg.Any<Execution>(), Arg.Any<CancellationToken>())
            .Returns(callInfo =>
            {
                var execution = callInfo.Arg<Execution>();
                execution.Id = executionId;
                execution.Timestamp = DateTime.UtcNow;
                return execution;
            });

        var handler = new CleanWithInputsCommandHandler(executionsRepositoryMock);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(executionId);
        result.Commands.Should().Be(commands.Length);
        result.Result.Should().Be(10); // initial position + 1/10 skipped
        result.DurationS.Should().BeGreaterThan(0);
    }
}