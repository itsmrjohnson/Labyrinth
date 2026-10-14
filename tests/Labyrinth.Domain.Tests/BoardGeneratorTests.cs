using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class BoardGeneratorTests
{
    [Fact]
    public void GetMovableTileDefinitions_ReturnsThirtyFourDefinitions()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetMovableTileDefinitions();

        Assert.Equal(34, definitions.Count);
    }

    [Fact]
    public void GetMovableTileDefinitions_ReturnsFifteenCorners()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetMovableTileDefinitions();

        Assert.Equal(
            15,
            definitions.Count(definition =>
                definition.Type == TileType.Corner));
    }

    [Fact]
    public void GetMovableTileDefinitions_ReturnsThirteenStraights()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetMovableTileDefinitions();

        Assert.Equal(
            13,
            definitions.Count(definition =>
                definition.Type == TileType.Straight));
    }

    [Fact]
    public void GetMovableTileDefinitions_ReturnsSixTJunctions()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetMovableTileDefinitions();

        Assert.Equal(
            6,
            definitions.Count(definition =>
                definition.Type == TileType.TJunction));
    }

    [Fact]
    public void GetFixedTileDefinitions_ReturnsSixteenFixedTileDefinitions()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetFixedTileDefinitions();

        Assert.Equal(16, definitions.Count);
    }

    [Fact]
    public void GetFixedTileDefinitions_ReturnsFixedTilesAtExpectedPositions()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetFixedTileDefinitions();

        var expectedPositions = new[]
        {
            new Position(0, 0),
            new Position(0, 2),
            new Position(0, 4),
            new Position(0, 6),
            new Position(2, 0),
            new Position(2, 2),
            new Position(2, 4),
            new Position(2, 6),
            new Position(4, 0),
            new Position(4, 2),
            new Position(4, 4),
            new Position(4, 6),
            new Position(6, 0),
            new Position(6, 2),
            new Position(6, 4),
            new Position(6, 6)
        };

        foreach (var position in expectedPositions)
        {
            Assert.Contains(
                definitions,
                definition => definition.Position == position);
        }
    }

    [Fact]
    public void GetFixedTileDefinitions_DoesNotContainDuplicatePositions()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetFixedTileDefinitions();

        var positions = definitions
            .Select(definition => definition.Position)
            .ToList();

        Assert.Equal(
            positions.Count,
            positions.Distinct().Count());
    }

    [Fact]
    public void GetFixedTileDefinitions_ReturnsExpectedFixedTiles()
    {
        var generator = new BoardGenerator();

        var definitions = generator.GetFixedTileDefinitions();

        var expectedDefinitions = new[]
        {
            new FixedTileDefinition(
                new Position(0, 0),
                TileType.Corner,
                TileRotation.Ninety),

            new FixedTileDefinition(
                new Position(0, 2),
                TileType.TJunction,
                TileRotation.OneEighty),

            new FixedTileDefinition(
                new Position(0, 4),
                TileType.TJunction,
                TileRotation.OneEighty),

            new FixedTileDefinition(
                new Position(0, 6),
                TileType.Corner,
                TileRotation.OneEighty),

            new FixedTileDefinition(
                new Position(2, 0),
                TileType.TJunction,
                TileRotation.Ninety),

            new FixedTileDefinition(
                new Position(2, 2),
                TileType.TJunction,
                TileRotation.Ninety),

            new FixedTileDefinition(
                new Position(2, 4),
                TileType.TJunction,
                TileRotation.OneEighty),

            new FixedTileDefinition(
                new Position(2, 6),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new FixedTileDefinition(
                new Position(4, 0),
                TileType.TJunction,
                TileRotation.Ninety),

            new FixedTileDefinition(
                new Position(4, 2),
                TileType.TJunction,
                TileRotation.Zero),

            new FixedTileDefinition(
                new Position(4, 4),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new FixedTileDefinition(
                new Position(4, 6),
                TileType.TJunction,
                TileRotation.TwoSeventy),

            new FixedTileDefinition(
                new Position(6, 0),
                TileType.Corner,
                TileRotation.Zero),

            new FixedTileDefinition(
                new Position(6, 2),
                TileType.TJunction,
                TileRotation.Zero),

            new FixedTileDefinition(
                new Position(6, 4),
                TileType.TJunction,
                TileRotation.Zero),

            new FixedTileDefinition(
                new Position(6, 6),
                TileType.Corner,
                TileRotation.TwoSeventy)
        };

        foreach (var expectedDefinition in expectedDefinitions)
        {
            Assert.Contains(expectedDefinition, definitions);
        }
    }

    [Fact]
    public void Generate_ReturnsSevenBySevenBoard()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                Assert.NotNull(
                    board.GetTile(new Position(row, column)));
            }
        }
    }

    [Fact]
    public void Generate_ContainsSixteenFixedTiles()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        var fixedTileCount = 0;

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var tile = board.GetTile(
                    new Position(row, column));

                if (tile.Movement == TileMovement.Fixed)
                {
                    fixedTileCount++;
                }
            }
        }

        Assert.Equal(16, fixedTileCount);
    }

    [Fact]
    public void Generate_ContainsThirtyThreeMovableTiles()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        var movableTileCount = 0;

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var tile = board.GetTile(
                    new Position(row, column));

                if (tile.Movement == TileMovement.Movable)
                {
                    movableTileCount++;
                }
            }
        }

        Assert.Equal(33, movableTileCount);
    }

    [Fact]
    public void Generate_ReturnsMovableSpareTile()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        Assert.Equal(
            TileMovement.Movable,
            board.SpareTile.Movement);
    }

    [Fact]
    public void Generate_ContainsFifteenMovableCornerTiles()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        Assert.Equal(
            15,
            CountMovableTiles(board, TileType.Corner));
    }

    [Fact]
    public void Generate_ContainsThirteenMovableStraightTiles()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        Assert.Equal(
            13,
            CountMovableTiles(board, TileType.Straight));
    }

    [Fact]
    public void Generate_ContainsSixMovableTJunctionTiles()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        Assert.Equal(
            6,
            CountMovableTiles(board, TileType.TJunction));
    }

    [Fact]
    public void Generate_PlacesFixedTilesAtExpectedPositions()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();
        var definitions = generator.GetFixedTileDefinitions();

        foreach (var definition in definitions)
        {
            var tile = board.GetTile(definition.Position);

            Assert.Equal(TileMovement.Fixed, tile.Movement);
            Assert.Equal(definition.Type, tile.Type);
            Assert.Equal(definition.Rotation, tile.Rotation);
        }
    }

    private static int CountMovableTiles(
        Board board,
        TileType type)
    {
        var count = 0;

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var tile = board.GetTile(
                    new Position(row, column));

                if (tile.Movement == TileMovement.Movable &&
                    tile.Type == type)
                {
                    count++;
                }
            }
        }

        if (board.SpareTile.Type == type)
        {
            count++;
        }

        return count;
    }

    [Fact]
    public void Generate_AssignsValidRotationToEveryMovableTile()
    {
        var generator = new BoardGenerator();

        var board = generator.Generate();

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var tile = board.GetTile(
                    new Position(row, column));

                if (tile.Movement != TileMovement.Movable)
                {
                    continue;
                }

                var validRotations =
                    TileRotationRules.GetValidRotations(tile.Type);

                Assert.Contains(tile.Rotation, validRotations);
            }
        }

        var spareValidRotations =
            TileRotationRules.GetValidRotations(board.SpareTile.Type);

        Assert.Contains(
            board.SpareTile.Rotation,
            spareValidRotations);
    }
}