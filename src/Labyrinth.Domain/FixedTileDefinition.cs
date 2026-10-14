namespace Labyrinth.Domain;

public readonly record struct FixedTileDefinition(
    Position Position,
    TileType Type,
    TileRotation Rotation);