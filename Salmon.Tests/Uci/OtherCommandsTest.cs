using Chess;
using Salmon.Engine;
using Salmon.Uci;

namespace Salmon.Tests.Uci;

public class UciTest
{
    [Fact]
    public void Respond_IsReady_ReturnsReadyOk()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        Utils.AssertUciResponse(ref board, ref engine, "isready", "readyok");
    }

    [Fact]
    public void Respond_GoInfinite_PlaysMove()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        var response = UciInterface.Respond(ref board, ref engine, "go infinite");
        Assert.StartsWith("bestmove", response);
        Assert.Single(board.ExecutedMoves);
    }

    [Fact]
    public void Respond_SetOption_SetsCorrectType()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);
        engine.Options["Life"] = 43;
        engine.Options["EnableFluxCapacitor"] = false;
        engine.Options["Type"] = "Meat";

        Utils.AssertUciResponse(ref board, ref engine, "setoption name Life value 42");
        Assert.Equal(42, engine.Options["Life"]);

        Utils.AssertUciResponse(ref board, ref engine, "setoption name EnableFluxCapacitor value true");
        Assert.Equal(true, engine.Options["EnableFluxCapacitor"]);

        Utils.AssertUciResponse(ref board, ref engine, "setoption name Type value Fish");
        Assert.Equal("Fish", engine.Options["Type"]);
    }
}
