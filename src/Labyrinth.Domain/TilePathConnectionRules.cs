namespace Labyrinth.Domain;

public static class TilePathConnectionRules
{

    public static IReadOnlyList<TilePathConnection> GetBaseConnections(TileType type)
    {
        return type switch
        {
            TileType.Straight => [TilePathConnection.North, TilePathConnection.South],
            TileType.Corner => [TilePathConnection.North, TilePathConnection.East],
            TileType.TJunction => [TilePathConnection.North, TilePathConnection.East, TilePathConnection.West],
            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }

    public static IReadOnlyList<TilePathConnection> GetConnections(
       TileType type,
       TileRotation rotation)
    {
        var baseConnections = GetBaseConnections(type);

        return rotation switch
        {
            TileRotation.Zero =>
                baseConnections,

            TileRotation.Ninety =>
                baseConnections
                    .Select(RotateClockwise)
                    .ToList(),

            TileRotation.OneEighty =>
                baseConnections
                    .Select(RotateClockwise)
                    .Select(RotateClockwise)
                    .ToList(),

            TileRotation.TwoSeventy =>
                baseConnections
                    .Select(RotateClockwise)
                    .Select(RotateClockwise)
                    .Select(RotateClockwise)
                    .ToList(),

            _ => throw new ArgumentOutOfRangeException(nameof(rotation), rotation, null)
        };
    }

    private static TilePathConnection RotateClockwise(
        TilePathConnection connection)
    {
        return connection switch
        {
            TilePathConnection.North => TilePathConnection.East,
            TilePathConnection.East => TilePathConnection.South,
            TilePathConnection.South => TilePathConnection.West,
            TilePathConnection.West => TilePathConnection.North,

            _ => throw new ArgumentOutOfRangeException(nameof(connection), connection, null)
        };
    }
}