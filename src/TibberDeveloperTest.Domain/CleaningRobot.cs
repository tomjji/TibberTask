using TibberDeveloperTest.Domain.Enums;
using TibberDeveloperTest.Domain.Exceptions;

namespace TibberDeveloperTest.Domain;

public class CleaningRobot(string id)
{
    private string Id { get; } = id;
    private State State { get; set; } = State.Unknown;
    private int X { get; set; }
    private int Y { get; set; }
    
    private readonly object _lock = new();
    public readonly HashSet<(int, int)> UniquePositions = []; // seems to perform better than ConcurrentDictionary
    public void Start() => State = State.Online;
    public void Stop() => State = State.Offline;

    public void SetInitialPosition(int x, int y)
    {
        X = x;
        Y = y;
        AddPosition(x, y);
    }

    private void AddPosition(int x, int y)
    {
        lock (_lock)
        {
            UniquePositions.Add((x, y));
        }
    }
    
    public void Move(Direction direction, int steps)
    {
        if (State != State.Online) throw new CleaningRobotNotOnlineException($"The cleaning robot Id = {Id} cannot move because it is not in the online state. State = {State}");
        
        // Feels like it should help when there's a possibility of having so many steps within a single command
        Parallel.For(0, steps, i =>
        {
            int newX;
            int newY;
            
            switch (direction)
            {
                case Direction.North:
                    newX = X;
                    newY = Y + i + 1;
                    break;
                case Direction.South:
                    newX = X;
                    newY = Y - i - 1;
                    break;
                case Direction.East:
                    newX = X + i + 1;
                    newY = Y;
                    break;
                case Direction.West:
                    newX = X - i - 1;
                    newY = Y;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            
            AddPosition(newX, newY);
        });
        
        // Set Final position
        switch (direction)
        {
            case Direction.North:
                Y += steps;
                break;
            case Direction.South:
                Y -= steps;
                break;
            case Direction.East:
                X += steps;
                break;
            case Direction.West:
                X -= steps;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
        }
    }
}