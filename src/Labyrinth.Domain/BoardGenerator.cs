namespace Labyrinth.Domain;

public class BoardGenerator
{
    private readonly Random _random;

    public BoardGenerator(Random? random = null)
    {
        _random = random ?? new Random();
    }

    public Board Generate()
    {
        var tiles = new Tile[Board.Size, Board.Size];

        // Place the 16 fixed tiles first.
        foreach (var definition in GetFixedTileDefinitions())
        {
            tiles[definition.Position.Row, definition.Position.Column] =
                new Tile(
                    definition.Position,
                    definition.Type,
                    TileMovement.Fixed,
                    definition.Rotation);
        }

        // Shuffle all 34 movable tile definitions.
        var movableDefinitions = GetMovableTileDefinitions().ToList();

        Shuffle(movableDefinitions);

        // Fill all remaining board positions with 33 movable tiles.
        var movableIndex = 0;

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                // A fixed tile already occupies this position.
                if (tiles[row, column] is not null)
                {
                    continue;
                }

                var definition = movableDefinitions[movableIndex];

                var rotation = GetRandomRotation(definition.Type);

                tiles[row, column] = definition.CreateTile(
                    new Position(row, column),
                    rotation);

                movableIndex++;
            }
        }

        // The remaining movable tile becomes the spare.
        var spareDefinition = movableDefinitions[movableIndex];

        var spareTile = spareDefinition.CreateTile(
            new Position(-1, -1),
            GetRandomRotation(spareDefinition.Type));

        return new Board(tiles, spareTile);
    }

    private void Shuffle(List<MovableTileDefinition> definitions)
    {
        for (var i = definitions.Count - 1; i > 0; i--)
        {
            var j = _random.Next(i + 1);

            (definitions[i], definitions[j]) =
                (definitions[j], definitions[i]);
        }
    }

    private TileRotation GetRandomRotation(TileType type)
    {
        var validRotations = TileRotationRules.GetValidRotations(type);

        return validRotations[_random.Next(validRotations.Count)];
    }

    public IReadOnlyList<MovableTileDefinition> GetMovableTileDefinitions()
    {
        return
        [
            // 15 Corner tiles
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),
            new(TileType.Corner),

            // 13 Straight tiles
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),
            new(TileType.Straight),

            // 6 T-junction tiles
            new(TileType.TJunction),
            new(TileType.TJunction),
            new(TileType.TJunction),
            new(TileType.TJunction),
            new(TileType.TJunction),
            new(TileType.TJunction)
        ];
    }

    public IReadOnlyList<FixedTileDefinition> GetFixedTileDefinitions()
    {
        return
        [
            new(
                new Position(0, 0),
                TileType.Corner,
                TileRotation.Ninety),

            new(
                new Position(0, 2),
                TileType.TJunction,
                TileRotation.OneEighty),

            new(
                new Position(0, 4),
                TileType.TJunction,
                TileRotation.OneEighty),

            new(
                new Position(0, 6),
                TileType.Corner,
                TileRotation.OneEighty),

            new(
                new Position(2, 0),
                TileType.TJunction,
                TileRotation.Ninety),

            new(
                new Position(2, 2),
                TileType.TJunction,
                TileRotation.Ninety),

            new(
                new Position(2, 4),
                TileType.TJunction,
                TileRotation.OneEighty),

            new(
                new Position(2, 6),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new(
                new Position(4, 0),
                TileType.TJunction,
                TileRotation.Ninety),

            new(
                new Position(4, 2),
                TileType.TJunction,
                TileRotation.Zero),

            new(
                new Position(4, 4),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new(
                new Position(4, 6),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new(
                new Position(6, 0),
                TileType.Corner,
                TileRotation.Zero),

            new(
                new Position(6, 2),
                TileType.TJunction,
                TileRotation.Zero),

            new(
                new Position(6, 4),
                TileType.TJunction,
                TileRotation.Zero),

            new(
                new Position(6, 6),
                TileType.Corner,
                TileRotation.TwoSeventy)
        ];
    }
}