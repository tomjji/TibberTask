using FluentAssertions;
using TibberDeveloperTest.Domain.Enums;

namespace TibberDeveloperTest.Domain.Tests;

public class CleaningRobotTests
{
    [Fact]
    public Task CleaningRobot_ShouldRecordAllUniquePositionsTouched_AfterSequenceOfCommands()
    {
        // Arrange
        var cleaningRobot = new CleaningRobot("test", 200000);

        var expectedPositions = new HashSet<(int, int)>
        {
            (10, 10), // Starting position
            (9, 10),  // After moving West 1
            (8, 10),  // After moving West 2
            (8, 11),  // After moving North 1
            (8, 12),  // After moving North 2
            (8, 13),  // After moving North 3
            (8, 14),  // After moving North 4
            (8, 15),  // After moving North 5
            (8, 14),  // After moving South 1
            (9, 14),  // After moving East 1
            (9, 13),  // After moving South 1
            (9, 12),  // After moving South 2
            (9, 11),  // After moving South 3
            (9, 12)   // after moving North 1
        };

        // Act
        cleaningRobot.Start();
        cleaningRobot.SetInitialPosition(10, 10);
        cleaningRobot.Move(10, 10, Direction.West, 2);
        cleaningRobot.Move(8, 10, Direction.North, 5);
        cleaningRobot.Move(8, 15, Direction.South, 1);
        cleaningRobot.Move(8,14, Direction.East, 1);
        cleaningRobot.Move(9, 14, Direction.South, 3);
        cleaningRobot.Move(9, 11, Direction.North, 1);
        
        // Assert
        cleaningRobot.UniquePoints.Should().Be(expectedPositions.Count);
        
        cleaningRobot.Stop();
        return Task.CompletedTask;
    }
}