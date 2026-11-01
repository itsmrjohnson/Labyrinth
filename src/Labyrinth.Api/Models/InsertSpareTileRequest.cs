using System.ComponentModel.DataAnnotations;
using Labyrinth.Domain;

namespace Labyrinth.Api.Models;

public sealed class InsertSpareTileRequest
{
    [EnumDataType(typeof(BoardInsertionLocation))]
    public BoardInsertionLocation Location { get; init; }

    [Range(1, 5)]
    public int Index { get; init; }

    [EnumDataType(typeof(TileRotation))]
    public TileRotation Rotation { get; init; }
}