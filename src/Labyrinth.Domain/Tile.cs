namespace Labyrinth.Domain;

public class Tile
{
    public Position Position { get; private set; }
    public TileType Type { get; }
    public TileRotation Rotation { get; private set; }
    public TileMovement Movement { get; }

    public Tile(
        Position position,
        TileType type,
        TileMovement movement)
        : this(position, type, movement, TileRotation.Zero)
    {
    }

    public Tile(
        Position position,
        TileType type,
        TileMovement movement,
        TileRotation rotation)
    {
        Position = position;
        Type = type;
        Movement = movement;
        Rotation = rotation;
    }

    public void RotateClockwise()
    {
        Rotation = Rotation switch
        {
            TileRotation.Zero => TileRotation.Ninety,
            TileRotation.Ninety => TileRotation.OneEighty,
            TileRotation.OneEighty => TileRotation.TwoSeventy,
            TileRotation.TwoSeventy => TileRotation.Zero,
            _ => throw new InvalidOperationException("Invalid tile rotation.")
        };
    }

    public void RotateCounterClockwise()
    {
        Rotation = Rotation switch
        {
            TileRotation.Zero => TileRotation.TwoSeventy,
            TileRotation.Ninety => TileRotation.Zero,
            TileRotation.OneEighty => TileRotation.Ninety,
            TileRotation.TwoSeventy => TileRotation.OneEighty,
            _ => throw new InvalidOperationException("Invalid tile rotation.")
        };
    }

    public IReadOnlyList<TilePathConnection> GetPathConnections()
    {
        return TilePathConnectionRules.GetConnections(Type, Rotation);
    }

    internal void MoveTo(Position position)
    {
        Position = position;
    }
}