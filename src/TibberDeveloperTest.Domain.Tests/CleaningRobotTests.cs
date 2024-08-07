using FluentAssertions;
using TibberDeveloperTest.Domain.Enums;

namespace TibberDeveloperTest.Domain.Tests;

public class CleaningRobotTests
{
    [Fact]
    public void CleaningRobot_ShouldRecordAllUniquePositionsTouched_AfterSequenceOfCommands()
    {
        // Arrange
        var cleaningRobot = new CleaningRobot("test");
        
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
        cleaningRobot.Move(Direction.West, 2);
        cleaningRobot.Move(Direction.North, 5);
        cleaningRobot.Move(Direction.South, 1);
        cleaningRobot.Move(Direction.East, 1);
        cleaningRobot.Move(Direction.South, 3);
        cleaningRobot.Move(Direction.North, 1);
        cleaningRobot.Stop();
        
        // Assert
        cleaningRobot.UniquePositions.Count.Should().Be(expectedPositions.Count);
        cleaningRobot.UniquePositions.Should().BeEquivalentTo(expectedPositions);
    }
}