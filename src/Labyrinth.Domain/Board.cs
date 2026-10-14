namespace Labyrinth.Domain;

public class Board
{
    public const int Size = 7;

    private readonly Tile[,] _tiles;

    public Tile SpareTile { get; private set; }

    public Board(Tile[,] tiles, Tile spareTile)
    {
        ValidateTiles(tiles);
        ValidateSpareTile(spareTile);

        _tiles = tiles;
        SpareTile = spareTile;
    }

    public Tile GetTile(Position position)
    {
        if (position.Row < 0 || position.Row >= Size ||
            position.Column < 0 || position.Column >= Size)
        {
            throw new ArgumentOutOfRangeException(nameof(position));
        }

        return _tiles[position.Row, position.Column];
    }

    public void InsertSpareTile(
     BoardInsertionLocation location,
     int index)
    {
        var outgoingTile = ValidateSpareTileInsertion(
            location,
            index);

        switch (location)
        {
            case BoardInsertionLocation.Left:
                InsertFromLeft(index);
                break;

            case BoardInsertionLocation.Right:
                InsertFromRight(index);
                break;

            case BoardInsertionLocation.Top:
                InsertFromTop(index);
                break;

            case BoardInsertionLocation.Bottom:
                InsertFromBottom(index);
                break;

            default:
                throw new ArgumentOutOfRangeException(
                    nameof(location),
                    location,
                    "Invalid board insertion location.");
        }

        outgoingTile.MoveTo(new Position(-1, -1));
        SpareTile = outgoingTile;
    }

    public IReadOnlyList<Position> GetReachablePositions(Position position)
    {
        GetTile(position);

        var visited = new HashSet<Position>
    {
        position
    };

        var positionsToVisit = new Queue<Position>();
        positionsToVisit.Enqueue(position);

        while (positionsToVisit.Count > 0)
        {
            var currentPosition = positionsToVisit.Dequeue();

            foreach (var connection in GetTile(currentPosition).GetPathConnections())
            {
                var neighbourPosition = GetNeighbourPosition(
                    currentPosition,
                    connection);

                if (neighbourPosition is null)
                {
                    continue;
                }

                if (visited.Contains(neighbourPosition.Value))
                {
                    continue;
                }

                var neighbourTile = GetTile(neighbourPosition.Value);

                if (!neighbourTile.GetPathConnections()
                    .Contains(GetOppositeConnection(connection)))
                {
                    continue;
                }

                visited.Add(neighbourPosition.Value);
                positionsToVisit.Enqueue(neighbourPosition.Value);
            }
        }

        return visited.ToList();
    }

    public void MovePlayer(Player player, Position destination)
    {
        ArgumentNullException.ThrowIfNull(player);

        var reachablePositions =
            GetReachablePositions(player.Position);

        if (!reachablePositions.Contains(destination))
        {
            throw new InvalidOperationException(
                "Player cannot move to the requested position.");
        }

        player.MoveTo(destination);
    }

    private void InsertFromLeft(int row)
    {
        var insertedTile = SpareTile;

        for (var column = Size - 1; column > 0; column--)
        {
            var tile = _tiles[row, column - 1];

            _tiles[row, column] = tile;
            tile.MoveTo(new Position(row, column));
        }

        _tiles[row, 0] = insertedTile;
        insertedTile.MoveTo(new Position(row, 0));
    }

    private void InsertFromRight(int row)
    {
        var insertedTile = SpareTile;

        for (var column = 0; column < Size - 1; column++)
        {
            var tile = _tiles[row, column + 1];

            _tiles[row, column] = tile;
            tile.MoveTo(new Position(row, column));
        }

        _tiles[row, Size - 1] = insertedTile;
        insertedTile.MoveTo(new Position(row, Size - 1));
    }

    private void InsertFromTop(int column)
    {
        var insertedTile = SpareTile;

        for (var row = Size - 1; row > 0; row--)
        {
            var tile = _tiles[row - 1, column];

            _tiles[row, column] = tile;
            tile.MoveTo(new Position(row, column));
        }

        _tiles[0, column] = insertedTile;
        insertedTile.MoveTo(new Position(0, column));
    }

    private void InsertFromBottom(int column)
    {
        var insertedTile = SpareTile;

        for (var row = 0; row < Size - 1; row++)
        {
            var tile = _tiles[row + 1, column];

            _tiles[row, column] = tile;
            tile.MoveTo(new Position(row, column));
        }

        _tiles[Size - 1, column] = insertedTile;
        insertedTile.MoveTo(new Position(Size - 1, column));
    }

    private Tile GetOutgoingTile(
        BoardInsertionLocation location,
        int index)
    {
        return location switch
        {
            BoardInsertionLocation.Left =>
                _tiles[index, Size - 1],

            BoardInsertionLocation.Right =>
                _tiles[index, 0],

            BoardInsertionLocation.Top =>
                _tiles[Size - 1, index],

            BoardInsertionLocation.Bottom =>
                _tiles[0, index],

            _ => throw new ArgumentOutOfRangeException(
                nameof(location),
                location,
                "Invalid board insertion location.")
        };
    }

    private static void ValidateInsertionIndex(int index)
    {
        if (index is not (1 or 3 or 5))
        {
            throw new ArgumentOutOfRangeException(
                nameof(index),
                "Insertion index must be 1, 3, or 5.");
        }
    }

    private static void ValidateTiles(Tile[,] tiles)
    {
        if (tiles.GetLength(0) != Size || tiles.GetLength(1) != Size)
        {
            throw new ArgumentException(
                "Board must contain 7x7 tiles.",
                nameof(tiles));
        }

        for (var row = 0; row < Size; row++)
        {
            for (var column = 0; column < Size; column++)
            {
                var tile = tiles[row, column];

                if (tile is null)
                {
                    throw new ArgumentException(
                        $"Tile at ({row}, {column}) cannot be null.",
                        nameof(tiles));
                }

                if (tile.Position != new Position(row, column))
                {
                    throw new ArgumentException(
                        $"Tile at ({row}, {column}) has an incorrect position.",
                        nameof(tiles));
                }
            }
        }
    }

    private static void ValidateSpareTile(Tile spareTile)
    {
        if (spareTile.Movement != TileMovement.Movable)
        {
            throw new ArgumentException(
                "Spare tile must be movable.",
                nameof(spareTile));
        }
    }

    private static Position? GetNeighbourPosition(
    Position position,
    TilePathConnection connection)
    {
        return connection switch
        {
            TilePathConnection.North =>
                position.Row > 0
                    ? new Position(position.Row - 1, position.Column)
                    : null,

            TilePathConnection.East =>
                position.Column < Size - 1
                    ? new Position(position.Row, position.Column + 1)
                    : null,

            TilePathConnection.South =>
                position.Row < Size - 1
                    ? new Position(position.Row + 1, position.Column)
                    : null,

            TilePathConnection.West =>
                position.Column > 0
                    ? new Position(position.Row, position.Column - 1)
                    : null,

            _ => throw new ArgumentOutOfRangeException(
                nameof(connection),
                connection,
                "Invalid tile path connection.")
        };
    }

    private static TilePathConnection GetOppositeConnection(
        TilePathConnection connection)
    {
        return connection switch
        {
            TilePathConnection.North => TilePathConnection.South,
            TilePathConnection.East => TilePathConnection.West,
            TilePathConnection.South => TilePathConnection.North,
            TilePathConnection.West => TilePathConnection.East,

            _ => throw new ArgumentOutOfRangeException(
                nameof(connection),
                connection,
                "Invalid tile path connection.")
        };
    }

    internal Tile ValidateSpareTileInsertion(
    BoardInsertionLocation location,
    int index)
    {
        ValidateInsertionIndex(index);

        var outgoingTile = GetOutgoingTile(location, index);

        if (outgoingTile.Movement == TileMovement.Fixed)
        {
            throw new InvalidOperationException(
                "Cannot insert spare tile because a fixed tile would be pushed off the board.");
        }

        return outgoingTile;
    }
}