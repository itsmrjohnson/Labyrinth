using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class FixedTileDefinitionTests
{
    [Fact]
    public void FixedTileDefinition_WithSameValues_IsEqual()
    {
        var definition1 = new FixedTileDefinition(
            new Position(0, 0),
            TileType.Corner,
            TileRotation.Ninety);

        var definition2 = new FixedTileDefinition(
            new Position(0, 0),
            TileType.Corner,
            TileRotation.Ninety);

        Assert.Equal(definition1, definition2);
    }

    [Fact]
    public void FixedTileDefinition_WithDifferentPosition_IsNotEqual()
    {
        var definition1 = new FixedTileDefinition(
            new Position(0, 0),
            TileType.Corner,
            TileRotation.Ninety);

        var definition2 = new FixedTileDefinition(
            new Position(1, 1),
            TileType.Corner,
            TileRotation.Ninety);

        Assert.NotEqual(definition1, definition2);
    }
}