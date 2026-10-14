using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class PositionTests
{
    [Fact]
    public void Position_WithSameCoordinates_IsEqual()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(2, 3);

        Assert.Equal(position1, position2);
    }

    [Fact]
    public void Position_WithDifferentCoordinates_IsNotEqual()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(1, 1);

        Assert.NotEqual(position1, position2);
    }

    [Fact]
    public void Position_WithSameRowButDifferentColumn_IsNotEqual()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(2, 1);

        Assert.NotEqual(position1, position2);
    }

    [Fact]
    public void Position_WithDifferentRowButSameColumn_IsNotEqual()
    {
        var position1 = new Position(2, 3);
        var position2 = new Position(1, 3);

        Assert.NotEqual(position1, position2);
    }
}