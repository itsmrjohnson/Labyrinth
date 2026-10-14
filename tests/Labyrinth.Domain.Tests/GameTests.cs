using Labyrinth.Domain;
using Xunit;

namespace Labyrinth.Tests;

public class GameTests
{
    [Fact]
    public void Game_StartsWithFirstPlayer()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        Assert.Equal(
            players[0],
            game.CurrentPlayer);

        Assert.Equal(
            0,
            game.CurrentPlayerIndex);

        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);
    }

    [Fact]
    public void Game_WithNullBoard_ThrowsArgumentNullException()
    {
        var players = CreatePlayers(2);

        Assert.Throws<ArgumentNullException>(
            () => new Game(
                null!,
                players));
    }

    [Fact]
    public void Game_WithNullPlayers_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => new Game(
                CreateBoard(),
                null!));
    }

    [Fact]
    public void Game_WithOnePlayer_ThrowsArgumentException()
    {
        var players = CreatePlayers(1);

        Assert.Throws<ArgumentException>(
            () => new Game(
                CreateBoard(),
                players));
    }

    [Fact]
    public void Game_WithTwoPlayers_IsCreatedSuccessfully()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        Assert.Equal(
            2,
            game.Players.Count);

        Assert.Equal(
            players[0],
            game.Players[0]);

        Assert.Equal(
            players[1],
            game.Players[1]);
    }

    [Fact]
    public void Game_WithFourPlayers_IsCreatedSuccessfully()
    {
        var players = CreatePlayers(4);

        var game = new Game(
            CreateBoard(),
            players);

        Assert.Equal(
            4,
            game.Players.Count);
    }

    [Fact]
    public void Game_WithFivePlayers_ThrowsArgumentException()
    {
        var players = new[]
        {
            new Player(new Position(0, 0)),
            new Player(new Position(0, 6)),
            new Player(new Position(6, 0)),
            new Player(new Position(6, 6)),
            new Player(new Position(1, 1))
        };

        Assert.Throws<ArgumentException>(
            () => new Game(
                CreateBoard(),
                players));
    }

    [Fact]
    public void Game_WithPlayersAtArbitraryPositions_IsCreatedSuccessfully()
    {
        var players = new[]
        {
            new Player(new Position(1, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        Assert.Equal(
            players[0],
            game.Players[0]);

        Assert.Equal(
            players[1],
            game.Players[1]);
    }

    [Fact]
    public void EndTurn_BeforeInsertion_ThrowsInvalidOperationException()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        Assert.Throws<InvalidOperationException>(
            () => game.EndTurn());
    }

    [Fact]
    public void EndTurn_AfterInsertion_MovesToNextPlayer()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        game.EndTurn();

        Assert.Equal(
            1,
            game.CurrentPlayerIndex);

        Assert.Equal(
            players[1],
            game.CurrentPlayer);

        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);
    }

    [Fact]
    public void EndTurn_FromLastPlayer_ReturnsToFirstPlayer()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        game.EndTurn();

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        game.EndTurn();

        Assert.Equal(
            0,
            game.CurrentPlayerIndex);

        Assert.Equal(
            players[0],
            game.CurrentPlayer);

        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);
    }

    [Fact]
    public void MoveCurrentPlayer_BeforeInsertion_ThrowsInvalidOperationException()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        Assert.Throws<InvalidOperationException>(
            () => game.MoveCurrentPlayer(
                new Position(0, 1)));
    }

    [Fact]
    public void MoveCurrentPlayer_ToReachablePosition_MovesPlayer()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        game.MoveCurrentPlayer(
            new Position(0, 6));

        Assert.Equal(
            new Position(0, 6),
            players[0].Position);
    }

    [Fact]
    public void MoveCurrentPlayer_ToUnreachablePosition_ThrowsInvalidOperationException()
    {
        var players = CreatePlayers(2);

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Throws<InvalidOperationException>(
            () => game.MoveCurrentPlayer(
                new Position(1, 0)));
    }

    [Fact]
    public void RotateSpareTileClockwise_DuringTileInsertion_RotatesTile()
    {
        var board = CreateBoard();

        var game = new Game(
            board,
            CreatePlayers());

        game.RotateSpareTileClockwise();

        Assert.Equal(
            TileRotation.OneEighty,
            board.SpareTile.Rotation);
    }

    [Fact]
    public void RotateSpareTileCounterClockwise_DuringTileInsertion_RotatesTile()
    {
        var board = CreateBoard();

        var game = new Game(
            board,
            CreatePlayers());

        game.RotateSpareTileCounterClockwise();

        Assert.Equal(
            TileRotation.Zero,
            board.SpareTile.Rotation);
    }

    [Fact]
    public void RotateSpareTileClockwise_AfterInsertion_ThrowsInvalidOperationException()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Throws<InvalidOperationException>(
            () => game.RotateSpareTileClockwise());
    }

    [Fact]
    public void RotateSpareTileCounterClockwise_AfterInsertion_ThrowsInvalidOperationException()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Throws<InvalidOperationException>(
            () => game.RotateSpareTileCounterClockwise());
    }

    [Fact]
    public void InsertSpareTile_DuringTileInsertion_ChangesPhaseToPlayerMovement()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(
            GameTurnPhase.PlayerMovement,
            game.TurnPhase);
    }

    [Fact]
    public void InsertSpareTile_AfterInsertion_ThrowsInvalidOperationException()
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Throws<InvalidOperationException>(
            () => game.InsertSpareTile(
                BoardInsertionLocation.Right,
                1));
    }

    [Fact]
    public void InsertSpareTile_MovesSpareTileIntoBoard()
    {
        var board = CreateBoard();

        var originalSpareTile =
            board.SpareTile;

        var game = new Game(
            board,
            CreatePlayers());

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(
            new Position(1, 0),
            originalSpareTile.Position);

        Assert.Equal(
            new Position(-1, -1),
            board.SpareTile.Position);
    }

    [Fact]
    public void InsertSpareTile_PlayerInInsertedRow_MovesWithRow()
    {
        var players = new[]
        {
            new Player(new Position(1, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(
            new Position(1, 2),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_PlayerOnOutgoingTile_MovesToOppositeSide()
    {
        var players = new[]
        {
            new Player(new Position(1, 6)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(
            new Position(1, 0),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_PlayerNotOnInsertedRow_DoesNotMove()
    {
        var players = new[]
        {
            new Player(new Position(2, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Left,
            1);

        Assert.Equal(
            new Position(2, 1),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_RightInsertion_PlayerInInsertedRow_MovesWithRow()
    {
        var players = new[]
        {
            new Player(new Position(1, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Right,
            1);

        Assert.Equal(
            new Position(1, 0),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_RightInsertion_PlayerAtLeftEdge_WrapsAround()
    {
        var players = new[]
        {
            new Player(new Position(1, 0)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Right,
            1);

        Assert.Equal(
            new Position(1, 6),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_TopInsertion_PlayerInInsertedColumn_MovesWithColumn()
    {
        var players = new[]
        {
            new Player(new Position(1, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Top,
            1);

        Assert.Equal(
            new Position(2, 1),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_TopInsertion_PlayerAtBottomEdge_WrapsAround()
    {
        var players = new[]
        {
            new Player(new Position(6, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Top,
            1);

        Assert.Equal(
            new Position(0, 1),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_BottomInsertion_PlayerInInsertedColumn_MovesWithColumn()
    {
        var players = new[]
        {
            new Player(new Position(5, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Bottom,
            1);

        Assert.Equal(
            new Position(4, 1),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_BottomInsertion_PlayerAtTopEdge_WrapsAround()
    {
        var players = new[]
        {
            new Player(new Position(0, 1)),
            new Player(new Position(3, 3))
        };

        var game = new Game(
            CreateBoard(),
            players);

        game.InsertSpareTile(
            BoardInsertionLocation.Bottom,
            1);

        Assert.Equal(
            new Position(6, 1),
            players[0].Position);
    }

    [Fact]
    public void InsertSpareTile_WhenInsertionIsInvalid_DoesNotChangeGame()
    {
        var players = new[]
        {
            new Player(new Position(1, 5)),
            new Player(new Position(3, 3))
        };

        var board = CreateBoardWithFixedOutgoingTile(
            BoardInsertionLocation.Left,
            1);

        var originalSpareTile =
            board.SpareTile;

        var game = new Game(
            board,
            players);

        Assert.Throws<InvalidOperationException>(
            () => game.InsertSpareTile(
                BoardInsertionLocation.Left,
                1));

        Assert.Equal(
            new Position(1, 5),
            players[0].Position);

        Assert.Equal(
            new Position(3, 3),
            players[1].Position);

        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);

        Assert.Equal(
            originalSpareTile,
            board.SpareTile);
    }

    [Fact]
    public void InsertSpareTile_AllValidIndices_AreAccepted()
    {
        foreach (var index in new[] { 1, 3, 5 })
        {
            var game = new Game(
                CreateBoard(),
                CreatePlayers());

            game.InsertSpareTile(
                BoardInsertionLocation.Left,
                index);

            Assert.Equal(
                GameTurnPhase.PlayerMovement,
                game.TurnPhase);
        }
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(6)]
    public void InsertSpareTile_InvalidIndex_ThrowsArgumentOutOfRangeException(
        int index)
    {
        var game = new Game(
            CreateBoard(),
            CreatePlayers());

        Assert.Throws<ArgumentOutOfRangeException>(
            () => game.InsertSpareTile(
                BoardInsertionLocation.Left,
                index));
    }

    private static Board CreateBoard()
    {
        var tiles = new Tile[Board.Size, Board.Size];

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var position = new Position(row, column);

                tiles[row, column] = new Tile(
                    position,
                    TileType.Straight,
                    TileMovement.Movable,
                    TileRotation.Ninety);
            }
        }

        var spareTile = new Tile(
            new Position(-1, -1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        return new Board(
            tiles,
            spareTile);
    }

    private static Board CreateBoardWithFixedOutgoingTile(
        BoardInsertionLocation location,
        int index)
    {
        var tiles = new Tile[Board.Size, Board.Size];

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var position = new Position(row, column);

                tiles[row, column] = new Tile(
                    position,
                    TileType.Straight,
                    TileMovement.Movable,
                    TileRotation.Ninety);
            }
        }

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

            _ => throw new ArgumentOutOfRangeException(
                nameof(location),
                location,
                "Invalid board insertion location.")
        };

        tiles[fixedPosition.Row, fixedPosition.Column] =
            new Tile(
                fixedPosition,
                TileType.Straight,
                TileMovement.Fixed,
                TileRotation.Ninety);

        var spareTile = new Tile(
            new Position(-1, -1),
            TileType.Straight,
            TileMovement.Movable,
            TileRotation.Ninety);

        return new Board(
            tiles,
            spareTile);
    }


    private static Player[] CreatePlayers(int count = 2)
    {
        return PlayerStartPositions.All
            .Take(count)
            .Select(position => new Player(position))
            .ToArray();
    }

    [Fact]
    public void CreateNew_WithTwoPlayers_CreatesGame()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            2);

        Assert.Equal(2, game.Players.Count);
    }

    [Fact]
    public void CreateNew_WithThreePlayers_CreatesGame()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            3);

        Assert.Equal(3, game.Players.Count);
    }

    [Fact]
    public void CreateNew_WithFourPlayers_CreatesGame()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            4);

        Assert.Equal(4, game.Players.Count);
    }

    [Fact]
    public void CreateNew_WithOnePlayer_ThrowsArgumentOutOfRangeException()
    {
        var boardGenerator = new BoardGenerator();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => Game.CreateNew(
                boardGenerator,
                1));
    }

    [Fact]
    public void CreateNew_WithFivePlayers_ThrowsArgumentOutOfRangeException()
    {
        var boardGenerator = new BoardGenerator();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => Game.CreateNew(
                boardGenerator,
                5));
    }

    [Fact]
    public void CreateNew_WithNullBoardGenerator_ThrowsArgumentNullException()
    {
        Assert.Throws<ArgumentNullException>(
            () => Game.CreateNew(
                null!,
                2));
    }

    [Fact]
    public void CreateNew_PlayersStartAtDesignatedPositions()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            4);

        Assert.Equal(
            PlayerStartPositions.All[0],
            game.Players[0].Position);

        Assert.Equal(
            PlayerStartPositions.All[1],
            game.Players[1].Position);

        Assert.Equal(
            PlayerStartPositions.All[2],
            game.Players[2].Position);

        Assert.Equal(
            PlayerStartPositions.All[3],
            game.Players[3].Position);
    }

    [Fact]
    public void CreateNew_StartsWithTileInsertionPhase()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            2);

        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);
    }

    [Fact]
    public void CreateNew_StartsWithFirstPlayer()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            2);

        Assert.Equal(
            0,
            game.CurrentPlayerIndex);

        Assert.Equal(
            game.Players[0],
            game.CurrentPlayer);
    }

    [Fact]
    public void CreateNew_GeneratesBoard()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            2);

        Assert.NotNull(game.Board);
        Assert.NotNull(game.Board.SpareTile);
    }

    [Fact]
    public void CreateNew_GeneratesMovableSpareTile()
    {
        var boardGenerator = new BoardGenerator();

        var game = Game.CreateNew(
            boardGenerator,
            2);

        Assert.Equal(
            TileMovement.Movable,
            game.Board.SpareTile.Movement);
    }
}