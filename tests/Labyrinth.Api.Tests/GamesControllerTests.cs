using System.Net.Http.Json;
using Labyrinth.Api.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Labyrinth.Domain;
using System.Net;

namespace Labyrinth.Api.Tests;

public class GamesControllerTests :
    IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public GamesControllerTests(
        WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]

    public async Task CreateGame_WithValidPlayerCount_ReturnsExpectedGame()
    {
        var request = new CreateGameRequest
        {
            PlayerCount = 2
        };

        var response = await _client.PostAsJsonAsync(
            "/api/games",
            request);

        var game = await response.Content.ReadFromJsonAsync<CreateGameResponse>();

        Assert.NotNull(game);
        Assert.Equal(0, game.CurrentPlayerIndex);
        Assert.Equal(
            GameTurnPhase.TileInsertion,
            game.TurnPhase);

        Assert.Equal(2, game.PlayerPositions.Count);

        Assert.Equal(
            new Position(0, 0),
            game.PlayerPositions[0]);

        Assert.Equal(
            new Position(0, 6),
            game.PlayerPositions[1]);
    }

    [Fact]
    public async Task CreateGame_WithInvalidPlayerCount_ReturnsBadRequest()
    {
        var request = new CreateGameRequest
        {
            PlayerCount = 1
        };

        var response = await _client.PostAsJsonAsync(
            "/api/games",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

    [Fact]
    public async Task CreateGame_WithFivePlayers_ReturnsBadRequest()
    {
        var request = new CreateGameRequest
        {
            PlayerCount = 5
        };

        var response = await _client.PostAsJsonAsync(
            "/api/games",
            request);

        Assert.Equal(
            HttpStatusCode.BadRequest,
            response.StatusCode);
    }

}