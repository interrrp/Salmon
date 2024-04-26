using Chess;
using Salmon.Engine;

namespace Salmon.Tests.Uci;

public class PositionCommandTest
{
    [Fact]
    public void Respond_PositionStartPos_ResetsBoard()
    {
        var board = new ChessBoard();
        board.Move(board.Moves()[0]);
        var engine = new SalmonEngine(board);

        Utils.AssertUciResponse(ref board, ref engine, "position startpos");
        Assert.Empty(board.ExecutedMoves);
    }

    [Fact]
    public void Respond_PositionFen_LoadsFen()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        var fen = "rnbqkbnr/pppppppp/8/8/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";
        Utils.AssertUciResponse(ref board, ref engine, $"position {fen}");
        Assert.Equal(fen, board.ToFen());
    }

    [Fact]
    public void Respond_PositionStartPosMoves_PlaysMoves()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        Utils.AssertUciResponse(ref board, ref engine, "position startpos moves e2e4 e7e5");
        Assert.Equal("1. e4 e5", board.ToPgn());
    }

    [Fact]
    public void Respond_PositionFenMoves_PlaysMoves()
    {
        var board = new ChessBoard();
        var engine = new SalmonEngine(board);

        // e4 e5
        var fen = "rnbqkbnr/pppp1ppp/8/4p3/4P3/8/PPPP1PPP/RNBQKBNR w KQkq - 0 1";
        Utils.AssertUciResponse(ref board, ref engine, $"position {fen} moves g1f3 d7d6");
        var pgn = board.ToPgn();

        Assert.Contains(fen, pgn);
        Assert.EndsWith("Nf3 d6", pgn);
    }
}
