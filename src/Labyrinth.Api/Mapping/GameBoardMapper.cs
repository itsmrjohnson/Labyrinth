using Labyrinth.Api.Models;
using Labyrinth.Domain;

namespace Labyrinth.Api.Mappings;

public static class GameBoardMapper
{
    public static IReadOnlyList<GameTileResponse> ToGameTileResponses(
        Board board)
    {
        var tiles = new List<GameTileResponse>();

        for (var row = 0; row < Board.Size; row++)
        {
            for (var column = 0; column < Board.Size; column++)
            {
                var tile = board.GetTile(new Position(row, column));

                tiles.Add(GameTileMapper.ToGameTileResponse(tile));
            }
        }

        return tiles;
    }
}