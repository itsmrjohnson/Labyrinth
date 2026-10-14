using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class TileTests
{
    [Fact]
    public void Tile_WhenInitialised_HasRotationOfZero()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable);

        Assert.Equal(TileRotation.Zero, tile.Rotation);
    }

    [Fact]
    public void Tile_WhenCreatedAsFixed_HasFixedMovement()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Fixed);

        Assert.Equal(TileMovement.Fixed, tile.Movement);
    }

    [Fact]
    public void Tile_WhenCreatedAsMovable_HasMovableMovement()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable);

        Assert.Equal(TileMovement.Movable, tile.Movement);
    }

    [Fact]
    public void Tile_WhenRotatedClockwise_HasRotationOfNinety()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable);

        tile.RotateClockwise();

        Assert.Equal(TileRotation.Ninety, tile.Rotation);
    }

    [Fact]
    public void Tile_WhenRotatedCounterClockwise_HasRotationOfTwoSeventy()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable);

        tile.RotateCounterClockwise();

        Assert.Equal(TileRotation.TwoSeventy, tile.Rotation);
    }

    [Fact]
    public void Tile_WhenRotatedClockwiseFourTimes_ReturnsToZero()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable);

        tile.RotateClockwise();
        tile.RotateClockwise();
        tile.RotateClockwise();
        tile.RotateClockwise();

        Assert.Equal(TileRotation.Zero, tile.Rotation);
    }

    [Fact]
    public void GetPathConnections_ForCornerRotatedNinety_ReturnsEastAndSouth()
    {
        var tile = new Tile(
            new Position(2, 3),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.Ninety);

        var connections = tile.GetPathConnections();

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.South, connections);
    }
}