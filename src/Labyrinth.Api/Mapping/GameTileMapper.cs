
using Labyrinth.Api.Models;
using Labyrinth.Domain;

namespace Labyrinth.Api.Mappings;

public static class GameTileMapper
{
    public static GameTileResponse ToGameTileResponse(Tile tile)
    {
        return new GameTileResponse
        {
            Row = tile.Position.Row,
            Column = tile.Position.Column,
            Type = tile.Type.ToString(),
            Rotation = tile.Rotation.ToString(),
            Movement = tile.Movement.ToString(),
            Connections = tile.GetPathConnections()
                .Select(connection => connection.ToString())
                .ToList()
        };
    }
}
