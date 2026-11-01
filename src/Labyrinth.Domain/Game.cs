namespace Labyrinth.Domain;

public class Game
{
    private readonly List<Player> _players;

    public Board Board { get; }

    public IReadOnlyList<Player> Players => _players;

    public int CurrentPlayerIndex { get; private set; }

    public Player CurrentPlayer => _players[CurrentPlayerIndex];

    public GameTurnPhase TurnPhase { get; private set; }

    public Game(
        Board board,
        IEnumerable<Player> players)
    {
        ArgumentNullException.ThrowIfNull(board);
        ArgumentNullException.ThrowIfNull(players);

        _players = players.ToList();

        if (_players.Count is < 2 or > 4)
        {
            throw new ArgumentException(
                "Game must contain between 2 and 4 players.",
                nameof(players));
        }

        Board = board;
        CurrentPlayerIndex = 0;
        TurnPhase = GameTurnPhase.TileInsertion;
    }

    public static Game CreateNew(
       BoardGenerator boardGenerator,
       int playerCount)
    {
        ArgumentNullException.ThrowIfNull(boardGenerator);

        if (playerCount is < 2 or > 4)
        {
            throw new ArgumentOutOfRangeException(
                nameof(playerCount),
                "Player count must be between 2 and 4.");
        }

        var board = boardGenerator.Generate();

        var players = PlayerStartPositions.All
            .Take(playerCount)
            .Select(position => new Player(position))
            .ToArray();

        return new Game(
            board,
            players);
    }


    public void RotateSpareTileClockwise()
    {
        EnsureTileInsertionPhase();

        Board.SpareTile.RotateClockwise();
    }

    public void RotateSpareTileCounterClockwise()
    {
        EnsureTileInsertionPhase();

        Board.SpareTile.RotateCounterClockwise();
    }

    public void InsertSpareTile(
     BoardInsertionLocation location,
     int index)
    {
        // Preserve the existing behaviour for callers that don't specify
        // a rotation.
        InsertSpareTile(location, index, Board.SpareTile.Rotation);
    }

    public void InsertSpareTile(
        BoardInsertionLocation location,
        int index,
        TileRotation rotation)
    {
        EnsureTileInsertionPhase();

        // Validate the insertion before changing the spare tile's rotation.
        Board.ValidateSpareTileInsertion(location, index);

        SetSpareTileRotation(rotation);

        Board.InsertSpareTile(location, index);

        MovePlayersWithInsertion(location, index);

        TurnPhase = GameTurnPhase.PlayerMovement;
    }

    public void MoveCurrentPlayer(Position destination)
    {
        if (TurnPhase != GameTurnPhase.PlayerMovement)
        {
            throw new InvalidOperationException(
                "The spare tile must be inserted before the player can move.");
        }

        Board.MovePlayer(
            CurrentPlayer,
            destination);
    }

    public void EndTurn()
    {
        if (TurnPhase != GameTurnPhase.PlayerMovement)
        {
            throw new InvalidOperationException(
                "The player must insert the spare tile before ending the turn.");
        }

        CurrentPlayerIndex =
            (CurrentPlayerIndex + 1) % _players.Count;

        TurnPhase = GameTurnPhase.TileInsertion;
    }

    public void SetSpareTileRotation(TileRotation rotation)
    {
        EnsureTileInsertionPhase();

        if (!Enum.IsDefined(rotation))
        {
            throw new ArgumentOutOfRangeException(
                nameof(rotation),
                rotation,
                "Invalid tile rotation.");
        }

        while (Board.SpareTile.Rotation != rotation)
        {
            Board.SpareTile.RotateClockwise();
        }
    }

    private void MovePlayersWithInsertion(
        BoardInsertionLocation location,
        int index)
    {
        foreach (var player in _players)
        {
            if (location == BoardInsertionLocation.Left &&
                player.Position.Row == index)
            {
                var newColumn =
                    player.Position.Column == Board.Size - 1
                        ? 0
                        : player.Position.Column + 1;

                player.MoveTo(
                    new Position(player.Position.Row, newColumn));
            }
            else if (location == BoardInsertionLocation.Right &&
                     player.Position.Row == index)
            {
                var newColumn =
                    player.Position.Column == 0
                        ? Board.Size - 1
                        : player.Position.Column - 1;

                player.MoveTo(
                    new Position(player.Position.Row, newColumn));
            }
            else if (location == BoardInsertionLocation.Top &&
                     player.Position.Column == index)
            {
                var newRow =
                    player.Position.Row == Board.Size - 1
                        ? 0
                        : player.Position.Row + 1;

                player.MoveTo(
                    new Position(newRow, player.Position.Column));
            }
            else if (location == BoardInsertionLocation.Bottom &&
                     player.Position.Column == index)
            {
                var newRow =
                    player.Position.Row == 0
                        ? Board.Size - 1
                        : player.Position.Row - 1;

                player.MoveTo(
                    new Position(newRow, player.Position.Column));
            }
        }
    }

    private void EnsureTileInsertionPhase()
    {
        if (TurnPhase != GameTurnPhase.TileInsertion)
        {
            throw new InvalidOperationException(
                "The spare tile has already been inserted for this turn.");
        }
    }
}