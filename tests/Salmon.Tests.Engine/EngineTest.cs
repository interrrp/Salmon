using Chess;
using Salmon.Engine;

namespace Salmon.Tests.Engine;

public class EngineTest
{
    [Fact]
    public void GetBestMove_FirstMove_ReturnsE4()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        var e4 = new Position("e4");
        var move = engine.GetBestMove().NewPosition;

        Assert.Equal(e4, move);
    }
}
