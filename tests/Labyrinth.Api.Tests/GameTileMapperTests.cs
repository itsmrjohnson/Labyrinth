using Labyrinth.Api.Mappings;
using Labyrinth.Api.Models;
using Labyrinth.Domain;

namespace Labyrinth.Api.Tests;

public class GameTileMapperTests
{
    [Fact]
    public void ToGameTileResponse_MapsTileProperties()
    {
        var tile = new Tile(
            new Position(3, 4),
            TileType.Corner,
            TileMovement.Movable,
            TileRotation.Ninety);

        GameTileResponse response = GameTileMapper.ToGameTileResponse(tile);

        Assert.Equal(3, response.Row);
        Assert.Equal(4, response.Column);
        Assert.Equal(nameof(TileType.Corner), response.Type);
        Assert.Equal(nameof(TileRotation.Ninety), response.Rotation);
        Assert.Equal(nameof(TileMovement.Movable), response.Movement);
        Assert.NotEmpty(response.Connections);
    }
}
