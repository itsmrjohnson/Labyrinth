using Labyrinth.Domain;

namespace Labyrinth.Domain.Tests;

public class BoardTests
{
    [Fact]
    public void GetTile_WithNegativeRow_ThrowsArgumentOutOfRangeException()
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            board.GetTile(new Position(-1, 0)));
    }

    [Fact]
    public void GetTile_WithRowOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            board.GetTile(new Position(Board.Size, 0)));
    }

    [Fact]
    public void GetTile_WithNegativeColumn_ThrowsArgumentOutOfRangeException()
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            board.GetTile(new Position(0, -1)));
    }

    [Fact]
    public void GetTile_WithColumnOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            board.GetTile(new Position(0, Board.Size)));
    }

    [Fact]
    public void GetTile_WithValidPosition_ReturnsTileAtRequestedPosition()
    {
        var board = CreateValidBoard();

        var tile = board.GetTile(new Position(2, 3));

        Assert.Equal(new Position(2, 3), tile.Position);
    }

    [Fact]
    public void Board_WithIncorrectDimensions_ThrowsArgumentException()
    {
        var tiles = new Tile[6, 7];
        var spareTile = CreateMovableTile();

        Assert.Throws<ArgumentException>(() =>
            new Board(tiles, spareTile));
    }

    [Fact]
    public void Board_WithNullTile_ThrowsArgumentException()
    {
        var tiles = CreateTiles();
        tiles[2, 3] = null!;
        var spareTile = CreateMovableTile();

        Assert.Throws<ArgumentException>(() =>
            new Board(tiles, spareTile));
    }

    [Fact]
    public void Board_WithTileAtIncorrectPosition_ThrowsArgumentException()
    {
        var tiles = CreateTiles();

        tiles[2, 3] = new Tile(
            new Position(1, 1),
            TileType.Straight,
            TileMovement.Movable);

        var spareTile = CreateMovableTile();

        Assert.Throws<ArgumentException>(() =>
            new Board(tiles, spareTile));
    }

    [Fact]
    public void Board_WithFixedSpareTile_ThrowsArgumentException()
    {
        var tiles = CreateTiles();

        var spareTile = new Tile(
            new Position(0, 0),
            TileType.Straight,
            TileMovement.Fixed);

        Assert.Throws<ArgumentException>(() =>
            new Board(tiles, spareTile));
    }

    [Fact]
    public void Board_WithMovableSpareTile_SetsSpareTile()
    {
        var tiles = CreateTiles();
        var spareTile = CreateMovableTile();

        var board = new Board(tiles, spareTile);

        Assert.Equal(spareTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromLeft_ShiftsRowRight()
    {
        var board = CreateValidBoard();

        var tile0 = board.GetTile(new Position(1, 0));
        var tile1 = board.GetTile(new Position(1, 1));
        var tile2 = board.GetTile(new Position(1, 2));
        var tile3 = board.GetTile(new Position(1, 3));
        var tile4 = board.GetTile(new Position(1, 4));
        var tile5 = board.GetTile(new Position(1, 5));
        var outgoingTile = board.GetTile(new Position(1, 6));
        var spareTile = board.SpareTile;

        board.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Same(spareTile, board.GetTile(new Position(1, 0)));
        Assert.Same(tile0, board.GetTile(new Position(1, 1)));
        Assert.Same(tile1, board.GetTile(new Position(1, 2)));
        Assert.Same(tile2, board.GetTile(new Position(1, 3)));
        Assert.Same(tile3, board.GetTile(new Position(1, 4)));
        Assert.Same(tile4, board.GetTile(new Position(1, 5)));
        Assert.Same(tile5, board.GetTile(new Position(1, 6)));

        Assert.Same(outgoingTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromRight_ShiftsRowLeft()
    {
        var board = CreateValidBoard();

        var outgoingTile = board.GetTile(new Position(1, 0));
        var tile1 = board.GetTile(new Position(1, 1));
        var tile2 = board.GetTile(new Position(1, 2));
        var tile3 = board.GetTile(new Position(1, 3));
        var tile4 = board.GetTile(new Position(1, 4));
        var tile5 = board.GetTile(new Position(1, 5));
        var tile6 = board.GetTile(new Position(1, 6));
        var spareTile = board.SpareTile;

        board.InsertSpareTile(
            BoardInsertionLocation.Right,
            1);

        Assert.Same(tile1, board.GetTile(new Position(1, 0)));
        Assert.Same(tile2, board.GetTile(new Position(1, 1)));
        Assert.Same(tile3, board.GetTile(new Position(1, 2)));
        Assert.Same(tile4, board.GetTile(new Position(1, 3)));
        Assert.Same(tile5, board.GetTile(new Position(1, 4)));
        Assert.Same(tile6, board.GetTile(new Position(1, 5)));
        Assert.Same(spareTile, board.GetTile(new Position(1, 6)));

        Assert.Same(outgoingTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromTop_ShiftsColumnDown()
    {
        var board = CreateValidBoard();

        var tile0 = board.GetTile(new Position(0, 1));
        var tile1 = board.GetTile(new Position(1, 1));
        var tile2 = board.GetTile(new Position(2, 1));
        var tile3 = board.GetTile(new Position(3, 1));
        var tile4 = board.GetTile(new Position(4, 1));
        var tile5 = board.GetTile(new Position(5, 1));
        var outgoingTile = board.GetTile(new Position(6, 1));
        var spareTile = board.SpareTile;

        board.InsertSpareTile(
            BoardInsertionLocation.Top,
            1);

        Assert.Same(spareTile, board.GetTile(new Position(0, 1)));
        Assert.Same(tile0, board.GetTile(new Position(1, 1)));
        Assert.Same(tile1, board.GetTile(new Position(2, 1)));
        Assert.Same(tile2, board.GetTile(new Position(3, 1)));
        Assert.Same(tile3, board.GetTile(new Position(4, 1)));
        Assert.Same(tile4, board.GetTile(new Position(5, 1)));
        Assert.Same(tile5, board.GetTile(new Position(6, 1)));

        Assert.Same(outgoingTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromBottom_ShiftsColumnUp()
    {
        var board = CreateValidBoard();

        var outgoingTile = board.GetTile(new Position(0, 1));
        var tile1 = board.GetTile(new Position(1, 1));
        var tile2 = board.GetTile(new Position(2, 1));
        var tile3 = board.GetTile(new Position(3, 1));
        var tile4 = board.GetTile(new Position(4, 1));
        var tile5 = board.GetTile(new Position(5, 1));
        var tile6 = board.GetTile(new Position(6, 1));
        var spareTile = board.SpareTile;

        board.InsertSpareTile(
            BoardInsertionLocation.Bottom,
            1);

        Assert.Same(tile1, board.GetTile(new Position(0, 1)));
        Assert.Same(tile2, board.GetTile(new Position(1, 1)));
        Assert.Same(tile3, board.GetTile(new Position(2, 1)));
        Assert.Same(tile4, board.GetTile(new Position(3, 1)));
        Assert.Same(tile5, board.GetTile(new Position(4, 1)));
        Assert.Same(tile6, board.GetTile(new Position(5, 1)));
        Assert.Same(spareTile, board.GetTile(new Position(6, 1)));

        Assert.Same(outgoingTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromLeft_UpdatesTilePositions()
    {
        var board = CreateValidBoard();

        var tile0 = board.GetTile(new Position(1, 0));
        var tile1 = board.GetTile(new Position(1, 1));
        var tile2 = board.GetTile(new Position(1, 2));
        var tile3 = board.GetTile(new Position(1, 3));
        var tile4 = board.GetTile(new Position(1, 4));
        var tile5 = board.GetTile(new Position(1, 5));
        var outgoingTile = board.GetTile(new Position(1, 6));
        var spareTile = board.SpareTile;

        board.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(new Position(1, 1), tile0.Position);
        Assert.Equal(new Position(1, 2), tile1.Position);
        Assert.Equal(new Position(1, 3), tile2.Position);
        Assert.Equal(new Position(1, 4), tile3.Position);
        Assert.Equal(new Position(1, 5), tile4.Position);
        Assert.Equal(new Position(1, 6), tile5.Position);

        Assert.Equal(new Position(1, 0), spareTile.Position);
        Assert.Equal(new Position(-1, -1), outgoingTile.Position);
        Assert.Same(outgoingTile, board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_FromLeft_DoesNotChangeOtherRows()
    {
        var board = CreateValidBoard();

        var unaffectedTile = board.GetTile(new Position(2, 2));

        board.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Same(
            unaffectedTile,
            board.GetTile(new Position(2, 2)));

        Assert.Equal(
            new Position(2, 2),
            unaffectedTile.Position);
    }

    [Fact]
    public void InsertSpareTile_FromTop_DoesNotChangeOtherColumns()
    {
        var board = CreateValidBoard();

        var unaffectedTile = board.GetTile(new Position(2, 2));

        board.InsertSpareTile(
            BoardInsertionLocation.Top,
            1);

        Assert.Same(
            unaffectedTile,
            board.GetTile(new Position(2, 2)));

        Assert.Equal(
            new Position(2, 2),
            unaffectedTile.Position);
    }

    [Fact]
    public void InsertSpareTile_WithFixedOutgoingTile_ThrowsInvalidOperationException()
    {
        var board = CreateBoardWithFixedOutgoingTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Throws<InvalidOperationException>(
            () => board.InsertSpareTile(
                BoardInsertionLocation.Left,
                1));
    }

    [Fact]
    public void InsertSpareTile_WithFixedOutgoingTile_DoesNotChangeBoard()
    {
        var board = CreateBoardWithFixedOutgoingTile(
            BoardInsertionLocation.Left,
            1);

        var originalSpareTile = board.SpareTile;
        var originalTile = board.GetTile(new Position(1, 0));

        Assert.Throws<InvalidOperationException>(
            () => board.InsertSpareTile(
                BoardInsertionLocation.Left,
                1));

        Assert.Same(originalSpareTile, board.SpareTile);

        Assert.Same(
            originalTile,
            board.GetTile(new Position(1, 0)));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public void InsertSpareTile_WithInvalidIndex_ThrowsArgumentOutOfRangeException(
        int index)
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => board.InsertSpareTile(
                BoardInsertionLocation.Left,
                index));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void InsertSpareTile_WithValidIndex_DoesNotThrow(int index)
    {
        var board = CreateValidBoard();

        var exception = Record.Exception(
            () => board.InsertSpareTile(
                BoardInsertionLocation.Left,
                index));

        Assert.Null(exception);
    }

    [Fact]
    public void InsertSpareTile_FromLeft_OnlyChangesSelectedRow()
    {
        var board = CreateValidBoard();

        var rowZeroTile = board.GetTile(new Position(0, 0));
        var rowTwoTile = board.GetTile(new Position(2, 0));
        var selectedRowTile = board.GetTile(new Position(1, 2));

        board.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Same(
            rowZeroTile,
            board.GetTile(new Position(0, 0)));

        Assert.Same(
            rowTwoTile,
            board.GetTile(new Position(2, 0)));

        Assert.NotSame(
            selectedRowTile,
            board.GetTile(new Position(1, 2)));
    }

    private static Board CreateValidBoard()
    {
        return new Board(
            CreateTiles(),
            CreateMovableTile());
    }

    private static Tile[,] CreateTiles()
    {
        var tiles = new Tile[Board.Size, Board.Size];

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                tiles[row, column] = new Tile(
                    new Position(row, column),
                    TileType.Straight,
                    TileMovement.Movable);
            }
        }

        return tiles;
    }

    private static Tile CreateMovableTile()
    {
        return new Tile(
            new Position(-1, -1),
            TileType.Straight,
            TileMovement.Movable);
    }

    private static Board CreateBoardWithFixedOutgoingTile(
        BoardInsertionLocation location,
        int index)
    {
        var tiles = CreateTiles();

        var fixedPosition = location switch
        {
            BoardInsertionLocation.Left =>
                new Position(index, Board.Size - 1),

            BoardInsertionLocation.Right =>
                new Position(index, 0),

            BoardInsertionLocation.Top =>
                new Position(Board.Size - 1, index),

            BoardInsertionLocation.Bottom =>
                new Position(0, index),

            _ => throw new ArgumentOutOfRangeException(nameof(location))
        };

        tiles[fixedPosition.Row, fixedPosition.Column] =
            new Tile(
                fixedPosition,
                TileType.Straight,
                TileMovement.Fixed);

        return new Board(
            tiles,
            CreateMovableTile());
    }

    [Fact]
    public void GetReachablePositions_WithMatchingNorthConnection_ReturnsNorthPosition()
    {
        var tiles = CreateTiles();

        tiles[2, 1] = new Tile(
            new Position(2, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[1, 1] = new Tile(
            new Position(1, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(2, 1));

        Assert.Contains(
            new Position(1, 1),
            reachablePositions);
    }

    [Fact]
    public void GetReachablePositions_WithNoMatchingNeighbourConnection_DoesNotReturnPosition()
    {
        var tiles = CreateTiles();

        tiles[2, 1] = new Tile(
            new Position(2, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[1, 1] = new Tile(
            new Position(1, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(2, 1));

        Assert.DoesNotContain(
            new Position(1, 1),
            reachablePositions);
    }

    [Fact]
    public void GetReachablePositions_WithMultipleMatchingConnections_ReturnsAllConnectedPositions()
    {
        var tiles = CreateTiles();

        tiles[3, 3] = new Tile(
            new Position(3, 3),
            TileType.TJunction,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[2, 3] = new Tile(
            new Position(2, 3),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[3, 4] = new Tile(
            new Position(3, 4),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        tiles[3, 2] = new Tile(
            new Position(3, 2),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        // Break the paths immediately beyond the three connected neighbours.

        tiles[1, 3] = new Tile(
            new Position(1, 3),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        tiles[3, 5] = new Tile(
            new Position(3, 5),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[3, 1] = new Tile(
            new Position(3, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(3, 3));

        Assert.Equal(4, reachablePositions.Count);

        Assert.Contains(
            new Position(3, 3),
            reachablePositions);

        Assert.Contains(
            new Position(2, 3),
            reachablePositions);

        Assert.Contains(
            new Position(3, 4),
            reachablePositions);

        Assert.Contains(
            new Position(3, 2),
            reachablePositions);
    }

    [Fact]
    public void GetReachablePositions_WithConnectionOutsideBoard_DoesNotReturnOutsidePosition()
    {
        var tiles = CreateTiles();

        tiles[0, 3] = new Tile(
            new Position(0, 3),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(0, 3));

        Assert.DoesNotContain(
            new Position(-1, 3),
            reachablePositions);
    }

    [Fact]
    public void MovePlayer_ToReachablePosition_MovesPlayer()
    {
        var tiles = CreateTiles();

        tiles[2, 1] = new Tile(
            new Position(2, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[1, 1] = new Tile(
            new Position(1, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var player = new Player(new Position(2, 1));

        board.MovePlayer(
            player,
            new Position(1, 1));

        Assert.Equal(
            new Position(1, 1),
            player.Position);
    }

    [Fact]
    public void MovePlayer_ToUnreachablePosition_ThrowsInvalidOperationException()
    {
        var tiles = CreateTiles();

        tiles[2, 1] = new Tile(
            new Position(2, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Zero);

        tiles[1, 1] = new Tile(
            new Position(1, 1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var player = new Player(new Position(2, 1));

        Assert.Throws<InvalidOperationException>(() =>
            board.MovePlayer(
                player,
                new Position(1, 1)));

        Assert.Equal(
            new Position(2, 1),
            player.Position);
    }

    [Fact]
    public void Player_WithPositionOutsideBoard_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Player(new Position(-1, 0)));
    }

    [Fact]
    public void Player_WithPositionBeyondBoard_ThrowsArgumentOutOfRangeException()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Player(new Position(Board.Size, 0)));
    }

    [Fact]
    public void MovePlayer_WithNullPlayer_ThrowsArgumentNullException()
    {
        var board = CreateValidBoard();

        Assert.Throws<ArgumentNullException>(() =>
            board.MovePlayer(
                null!,
                new Position(1, 1)));
    }
    [Fact]
    public void GetReachablePositions_WithConnectedPath_ReturnsAllConnectedPositions()
    {
        var tiles = CreateTiles();

        for (var column = 0; column < Board.Size; column++)
        {
            tiles[1, column] = new Tile(
                new Position(1, column),
                TileType.Straight,
                TileMovement.Movable,
                TileRotation.Ninety);
        }

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(1, 1));

        Assert.Contains(new Position(1, 0), reachablePositions);
        Assert.Contains(new Position(1, 1), reachablePositions);
        Assert.Contains(new Position(1, 2), reachablePositions);
        Assert.Contains(new Position(1, 3), reachablePositions);
        Assert.Contains(new Position(1, 4), reachablePositions);
        Assert.Contains(new Position(1, 5), reachablePositions);
        Assert.Contains(new Position(1, 6), reachablePositions);
    }

    [Fact]
    public void GetReachablePositions_WithLoop_ReturnsEachPositionOnce()
    {
        var tiles = CreateTiles();

        tiles[2, 2] = new Tile(
            new Position(2, 2),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.Ninety);

        tiles[2, 3] = new Tile(
            new Position(2, 3),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.OneEighty);

        tiles[3, 3] = new Tile(
            new Position(3, 3),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.TwoSeventy);

        tiles[3, 2] = new Tile(
            new Position(3, 2),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.Zero);

        var board = new Board(
            tiles,
            CreateMovableTile());

        var reachablePositions =
            board.GetReachablePositions(new Position(2, 2));

        Assert.Equal(4, reachablePositions.Count);

        Assert.Contains(
            new Position(2, 2),
            reachablePositions);

        Assert.Contains(
            new Position(2, 3),
            reachablePositions);

        Assert.Contains(
            new Position(3, 3),
            reachablePositions);

        Assert.Contains(
            new Position(3, 2),
            reachablePositions);
    }


}