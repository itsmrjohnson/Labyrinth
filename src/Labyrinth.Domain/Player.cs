namespace Labyrinth.Domain;

public class Player
{
    public Position Position { get; private set; }

    public Player(Position position)
    {
        ValidatePosition(position);

        Position = position;
    }

    internal void MoveTo(Position position)
    {
        ValidatePosition(position);

        Position = position;
    }

    private static void ValidatePosition(Position position)
    {
        if (position.Row < 0 || position.Row >= Board.Size ||
            position.Column < 0 || position.Column >= Board.Size)
        {
            throw new ArgumentOutOfRangeException(
                nameof(position),
                "Player position must be inside the board.");
        }
    }
}