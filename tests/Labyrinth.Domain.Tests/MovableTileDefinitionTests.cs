using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class MovableTileDefinitionTests
{
    [Fact]
    public void MovableTileDefinition_WithSameType_IsEqual()
    {
        var definition1 = new MovableTileDefinition(TileType.Corner);
        var definition2 = new MovableTileDefinition(TileType.Corner);

        Assert.Equal(definition1, definition2);
    }

    [Fact]
    public void MovableTileDefinition_WithDifferentType_IsNotEqual()
    {
        var definition1 = new MovableTileDefinition(TileType.Corner);
        var definition2 = new MovableTileDefinition(TileType.Straight);

        Assert.NotEqual(definition1, definition2);
    }

    [Fact]
    public void CreateTile_CreatesMovableTileWithDefinitionValues()
    {
        var definition = new MovableTileDefinition(TileType.Corner);

        var tile = definition.CreateTile(
            new Position(2, 3),
            TileRotation.Ninety);

        Assert.Equal(new Position(2, 3), tile.Position);
        Assert.Equal(TileType.Corner, tile.Type);
        Assert.Equal(TileRotation.Ninety, tile.Rotation);
        Assert.Equal(TileMovement.Movable, tile.Movement);
    }
}