using TibberDeveloperTest.Domain.Enums;
using TibberDeveloperTest.Domain.Exceptions;

namespace TibberDeveloperTest.Domain;

public class CleaningRobot(string id, int gridSize) : IDisposable
{
    private readonly int _offset = gridSize / 2; // offset all x/y's to positive array index values
    private string Id { get; } = id;
    private State State { get; set; } = State.Unknown;
    private void AddPosition(int x, int y)
    {
        if (TouchPoint(x + _offset, y + _offset)) UniquePoints++;
    }
    //private readonly object _lock = new object();
    //public ConcurrentDictionary<(int x, int y), byte> UniquePositions = [];
    
    public int UniquePoints { get; set; }

    private ulong[] _bitArray = new ulong[(long)gridSize * gridSize / 64];
    public void Start() => State = State.Online;
    public void Stop()
    {
        State = State.Offline;
        Dispose();
    }

    public void SetInitialPosition(int x, int y) => AddPosition(x, y);

    public void Move(int startX, int startY, Direction direction, int steps)
    {
        if (State != State.Online)
            throw new CleaningRobotNotOnlineException(
                $"The cleaning robot Id = {Id} cannot move because it is not in the online state. State = {State}");
        
        for (var i = 0; i < steps; i++)
        {
            switch (direction)
            {
                case Direction.North: AddPosition(startX, startY + i + 1); break;
                case Direction.South: AddPosition(startX, startY - i - 1); break;
                case Direction.East: AddPosition(startX + i + 1, startY); break;
                case Direction.West: AddPosition(startX - i - 1, startY); break;
                default: throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
        }
    }
    
    private long GetBitIndex(long x, long y) => y * gridSize + x;

    private bool TouchPoint(int x, int y)
    {
        var index = GetBitIndex(x, y);
        var bitPosition = (int)(index % 64);
        var arrayIndex = index / 64;
        
        if ((_bitArray[arrayIndex] & (1UL << bitPosition)) == 0)
        {
            _bitArray[arrayIndex] |= (1UL << bitPosition);
            return true;
        }
        return false;
    }

    public void Dispose()
    {
        _bitArray = null!;
        UniquePoints = 0;
        GC.Collect();
    }
}