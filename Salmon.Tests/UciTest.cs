using Chess;

namespace Salmon.Tests;

public class UciTest
{
    [Fact]
    public void Respond_Uci_ReturnsUciOk()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        var response = Uci.Respond(ref board, ref engine, "uci");
        Assert.EndsWith("uciok\n", response);
    }

    [Fact]
    public void Respond_Uci_ReturnsId()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        var response = Uci.Respond(ref board, ref engine, "uci");
        var lines = response.Split("\n");

        Assert.Contains("id name", lines[0]);
        Assert.Contains("id author", lines[1]);
    }

    [Fact]
    public void Respond_Uci_ReturnsOptions()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);
        engine.Options["Life"] = 42;
        engine.Options["EnableFluxCapacitor"] = true;
        engine.Options["Type"] = "Fish";

        var response = Uci.Respond(ref board, ref engine, "uci");

        Assert.Contains("option name Life type spin default 42", response);
        Assert.Contains("option name EnableFluxCapacitor type check default true", response);
        Assert.Contains("option name Type type string default Fish", response);
    }

    [Fact]
    public void Respond_IsReady_ReturnsReadyOk()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        AssertUciResponse(ref board, ref engine, "isready", "readyok");
    }

    [Fact]
    public void Respond_PositionStartPos_ResetsBoard()
    {
        var board = new ChessBoard();
        board.Move(board.Moves()[0]);
        var engine = new Engine(board);

        AssertUciResponse(ref board, ref engine, "position startpos");
        Assert.Empty(board.ExecutedMoves);
    }

    [Fact]
    public void Respond_PositionFen_LoadsFen()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";
        AssertUciResponse(ref board, ref engine, $"position {fen}");
        Assert.Equal(fen, board.ToFen());
    }

    [Fact]
    public void Respond_PositionStartPosMoves_PlaysMoves()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        AssertUciResponse(ref board, ref engine, "position startpos moves e2e4 e7e5");
        Assert.Equal("1. e4 e5", board.ToPgn());
    }

    [Fact]
    public void Respond_PositionFenMoves_PlaysMoves()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        // e4 e5
        var fen = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";
        AssertUciResponse(ref board, ref engine, $"position {fen} moves g1f3 d7d6");
        var pgn = board.ToPgn();

        Assert.Contains(fen, pgn);
        Assert.EndsWith("Nf3 d6", pgn);
    }

    [Fact]
    public void Respond_GoInfinite_PlaysMove()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        var response = Uci.Respond(ref board, ref engine, "go infinite");
        Assert.StartsWith("bestmove", response);
        Assert.Single(board.ExecutedMoves);
    }

    [Fact]
    public void Respond_SetOptionDepth_SetsDepth()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        AssertUciResponse(ref board, ref engine, "setoption name Depth value 2");
        Assert.Equivalent(2, engine.Options["Depth"]);
    }

    private void AssertUciResponse(
        ref ChessBoard board, ref Engine engine,
        string command, params string[] expectedLines)
    {
        var response = Uci.Respond(ref board, ref engine, command);

        if (expectedLines.Length <= 0)
        {
            // Should be no response
            Assert.Empty(response);
            return;
        }

        var expected = string.Join("\n", expectedLines) + "\n";
        Assert.Equal(expected, response);
    }
}
