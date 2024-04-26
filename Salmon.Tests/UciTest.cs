using Chess;

namespace Salmon.Tests;

public class UciTest
{
    [Fact]
    public void Respond_Uci_ReturnsIdAndOk()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        var response = Uci.Respond(ref board, ref engine, "uci");
        Assert.StartsWith("id", response);
        Assert.EndsWith("uciok\n", response);
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
    public void Respond_SetDepth_SetsDepth()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        AssertUciResponse(ref board, ref engine, "setoption name Depth value 2");
        Assert.Equal(2, engine.Depth);
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