using Chess;

namespace Salmon.Tests;

public class UciTest
{
    [Fact]
    public void Respond_Uci_ReturnsIdAndOk()
    {
        var board = new ChessBoard();
        var engine = new Engine(board);

        AssertUciResponse(
            ref board, ref engine,
            "uci",
            $"id name {Engine.Name}",
            $"id author {Engine.Author}",
            "uciok"
        );
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
        AssertUciResponse(ref board, ref engine, $"position {fen} moves f2f4 f7f5");
        Assert.Equal("rnbqkbnr/pppp2pp/8/4pp2/4PP2/8/PPPP2PP/RNBQKBNR w KQkq f6 0 2", board.ToFen());
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
