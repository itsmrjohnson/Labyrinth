namespace Labyrinth.Domain;

public static class TileRotationRules
{
    public static IReadOnlyList<TileRotation> GetValidRotations(TileType type)
    {
        return type switch
        {
            TileType.Corner =>
            [
                TileRotation.Zero,
                TileRotation.Ninety,
                TileRotation.OneEighty,
                TileRotation.TwoSeventy
            ],

            TileType.TJunction =>
            [
                TileRotation.Zero,
                TileRotation.Ninety,
                TileRotation.OneEighty,
                TileRotation.TwoSeventy
            ],

            TileType.Straight =>
            [
                TileRotation.Zero,
                TileRotation.Ninety
            ],

            _ => throw new ArgumentOutOfRangeException(nameof(type), type, null)
        };
    }
}