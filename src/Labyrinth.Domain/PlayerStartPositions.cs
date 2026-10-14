namespace Labyrinth.Domain;

public static class PlayerStartPositions
{
    public static IReadOnlyList<Position> All { get; } =
    [
        new Position(0, 0),
        new Position(0, 6),
        new Position(6, 0),
        new Position(6, 6)
    ];

    public static bool IsValid(Position position)
    {
        return All.Contains(position);
    }
}