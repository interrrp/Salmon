using Chess;
using Salmon.Uci;

namespace Salmon.Tests.Uci;

public class LanParserTest
{
    [Fact]
    public void ParseFromLan_ValidLan_ReturnsMove()
    {
        var board = new ChessBoard();
        var lan = "e2e4";

        var move = board.ParseFromLan(lan);

        Assert.NotNull(move);
        Assert.Equal(new Position("e2"), move.OriginalPosition);
        Assert.Equal(new Position("e4"), move.NewPosition);
    }

    [Fact]
    public void MoveLan_ValidLan_MovesPiece()
    {
        var board = new ChessBoard();
        var lan = "e2e4";

        board.MoveLan(lan);

        Assert.Equal("1. e4", board.ToPgn());
    }
}

