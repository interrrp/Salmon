using Chess;
using Salmon.Engine;
using Salmon.Extensions;

namespace Salmon.Tests.Engine;

public class EvaluatorTest
{
    [Fact]
    public void Evaluate_WhiteWins_ReturnsBestScore()
    {
        // Bongcloud or whatever
        var board = ChessBoard.LoadFromFen("rnbq1bnr/ppppkppp/8/4Q3/4P3/8/PPPP1PPP/RNB1KBNR b KQ - 0 3");
        Assert.Equal(board.Evaluate(), PieceColor.White.BestScore());
    }

    [Fact]
    public void Evaluate_BlackWins_ReturnsBestScore()
    {
        // Classic Fool's Mate
        var board = ChessBoard.LoadFromFen("rnb1kbnr/pppp1ppp/8/4p3/6Pq/5P2/PPPPP2P/RNBQKBNR w KQkq - 1 3");
        Assert.Equal(board.Evaluate(), PieceColor.Black.BestScore());
    }
}
