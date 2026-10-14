using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class TileRotationTests
{
    [Fact]
    public void GetValidRotations_ForCorner_ReturnsAllFourRotations()
    {
        var rotations = TileRotationRules.GetValidRotations(TileType.Corner);

        Assert.Equal(
            [
                TileRotation.Zero,
                TileRotation.Ninety,
                TileRotation.OneEighty,
                TileRotation.TwoSeventy
            ],
            rotations);
    }

    [Fact]
    public void GetValidRotations_ForTJunction_ReturnsAllFourRotations()
    {
        var rotations = TileRotationRules.GetValidRotations(TileType.TJunction);

        Assert.Equal(
            [
                TileRotation.Zero,
                TileRotation.Ninety,
                TileRotation.OneEighty,
                TileRotation.TwoSeventy
            ],
            rotations);
    }

    [Fact]
    public void GetValidRotations_ForStraight_ReturnsZeroAndNinety()
    {
        var rotations = TileRotationRules.GetValidRotations(TileType.Straight);

        Assert.Equal(
            [
                TileRotation.Zero,
                TileRotation.Ninety
            ],
            rotations);
    }
}