namespace Labyrinth.Domain.Tests;

public class PlayerStartPositionsTests
{
    [Fact]
    public void IsValid_WithTopLeftPosition_ReturnsTrue()
    {
        Assert.True(
            PlayerStartPositions.IsValid(
                new Position(0, 0)));
    }

    [Fact]
    public void IsValid_WithTopRightPosition_ReturnsTrue()
    {
        Assert.True(
            PlayerStartPositions.IsValid(
                new Position(0, 6)));
    }

    [Fact]
    public void IsValid_WithBottomLeftPosition_ReturnsTrue()
    {
        Assert.True(
            PlayerStartPositions.IsValid(
                new Position(6, 0)));
    }

    [Fact]
    public void IsValid_WithBottomRightPosition_ReturnsTrue()
    {
        Assert.True(
            PlayerStartPositions.IsValid(
                new Position(6, 6)));
    }

    [Fact]
    public void IsValid_WithNonStartingPosition_ReturnsFalse()
    {
        Assert.False(
            PlayerStartPositions.IsValid(
                new Position(1, 1)));
    }

    [Fact]
    public void All_ContainsFourStartingPositions()
    {
        Assert.Equal(
            4,
            PlayerStartPositions.All.Count);
    }
}