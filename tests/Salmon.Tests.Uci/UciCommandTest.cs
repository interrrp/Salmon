using Chess;
using Salmon.Engine;
using Salmon.Uci;

namespace Salmon.Tests.Uci;

public class UciCommandTest
{
    [Fact]
    public void Respond_Uci_ReturnsUciOk()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        var response = UciInterface.Respond(ref board, ref engine, "uci");
        Assert.EndsWith("uciok\n", response);
    }

    [Fact]
    public void Respond_Uci_ReturnsId()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        var response = UciInterface.Respond(ref board, ref engine, "uci");
        var lines = response.Split("\n");

        Assert.Contains("id name", lines[0]);
        Assert.Contains("id author", lines[1]);
    }

    [Fact]
    public void Respond_Uci_ReturnsOptions()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);
        engine.Options["Life"] = 42;
        engine.Options["EnableFluxCapacitor"] = true;
        engine.Options["Type"] = "Fish";

        var response = UciInterface.Respond(ref board, ref engine, "uci");

        Assert.Contains($"option name Life type spin default 42 min {int.MinValue} max {int.MaxValue}", response);
        Assert.Contains("option name EnableFluxCapacitor type check default true", response);
        Assert.Contains("option name Type type string default Fish", response);
    }
}
