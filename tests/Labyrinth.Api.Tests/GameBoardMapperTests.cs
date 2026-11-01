using Labyrinth.Api.Mappings;
using Labyrinth.Domain;

namespace Labyrinth.Api.Tests;

public class GameBoardMapperTests
{
    [Fact]
    public void ToGameTileResponses_MapsEveryBoardPosition()
    {
        var board = new BoardGenerator(new Random(42)).Generate();

        var responses = GameBoardMapper.ToGameTileResponses(board);

        Assert.Equal(Board.Size * Board.Size, responses.Count);

        foreach (var response in responses)
        {
            Assert.Equal(
                board.GetTile(new Position(response.Row, response.Column)).Type.ToString(),
                response.Type);
        }
    }
}
