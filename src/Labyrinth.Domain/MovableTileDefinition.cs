namespace Labyrinth.Domain;

public readonly record struct MovableTileDefinition(
    TileType Type)
{
    public Tile CreateTile(Position position, TileRotation rotation)
    {
        return new Tile(
            position,
            Type,
            TileMovement.Movable,
            rotation);
    }
}