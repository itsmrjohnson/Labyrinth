using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class TilePathConnectionRulesTests
{
    [Fact]
    public void GetConnections_CornerWithZeroRotation_ReturnsNorthAndEast()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Corner,
            TileRotation.Zero);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.North, connections);
        Assert.Contains(TilePathConnection.East, connections);
    }

    [Fact]
    public void GetConnections_CornerWithNinetyRotation_ReturnsEastAndSouth()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Corner,
            TileRotation.Ninety);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.South, connections);
    }

    [Fact]
    public void GetConnections_CornerWithOneEightyRotation_ReturnsSouthAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Corner,
            TileRotation.OneEighty);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.South, connections);
        Assert.Contains(TilePathConnection.West, connections);
    }

    [Fact]
    public void GetConnections_CornerWithTwoSeventyRotation_ReturnsWestAndNorth()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Corner,
            TileRotation.TwoSeventy);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.West, connections);
        Assert.Contains(TilePathConnection.North, connections);
    }

    [Fact]
    public void GetConnections_StraightWithZeroRotation_ReturnsNorthAndSouth()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Straight,
            TileRotation.Zero);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.North, connections);
        Assert.Contains(TilePathConnection.South, connections);
    }

    [Fact]
    public void GetConnections_StraightWithNinetyRotation_ReturnsEastAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Straight,
            TileRotation.Ninety);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.West, connections);
    }

    [Fact]
    public void GetConnections_StraightWithOneEightyRotation_ReturnsNorthAndSouth()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Straight,
            TileRotation.OneEighty);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.North, connections);
        Assert.Contains(TilePathConnection.South, connections);
    }

    [Fact]
    public void GetConnections_StraightWithTwoSeventyRotation_ReturnsEastAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.Straight,
            TileRotation.TwoSeventy);

        Assert.Equal(2, connections.Count);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.West, connections);
    }

    [Fact]
    public void GetConnections_TJunctionWithZeroRotation_ReturnsNorthEastAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.TJunction,
            TileRotation.Zero);

        Assert.Equal(3, connections.Count);
        Assert.Contains(TilePathConnection.North, connections);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.West, connections);
    }

    [Fact]
    public void GetConnections_TJunctionWithNinetyRotation_ReturnsNorthEastAndSouth()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.TJunction,
            TileRotation.Ninety);

        Assert.Equal(3, connections.Count);
        Assert.Contains(TilePathConnection.East, connections);
        Assert.Contains(TilePathConnection.South, connections);
        Assert.Contains(TilePathConnection.North, connections);
    }

    [Fact]
    public void GetConnections_TJunctionWithOneEightyRotation_ReturnsEastSouthAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.TJunction,
            TileRotation.OneEighty);

        Assert.Equal(3, connections.Count);
        Assert.Contains(TilePathConnection.South, connections);
        Assert.Contains(TilePathConnection.West, connections);
        Assert.Contains(TilePathConnection.East, connections);
    }

    [Fact]
    public void GetConnections_TJunctionWithTwoSeventyRotation_ReturnsNorthSouthAndWest()
    {
        var connections = TilePathConnectionRules.GetConnections(
            TileType.TJunction,
            TileRotation.TwoSeventy);

        Assert.Equal(3, connections.Count);
        Assert.Contains(TilePathConnection.West, connections);
        Assert.Contains(TilePathConnection.North, connections);
        Assert.Contains(TilePathConnection.South, connections);
    }

    [Fact]
    public void GetConnections_WithInvalidTileType_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            TilePathConnectionRules.GetConnections(
                (TileType)999,
                TileRotation.Zero));
    }

    [Fact]
    public void GetConnections_WithInvalidRotation_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            TilePathConnectionRules.GetConnections(
                TileType.Corner,
                (TileRotation)999));
    }
}