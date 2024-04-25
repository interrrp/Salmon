using Chess;

namespace Salmon;

public class Engine(ChessBoard board, int depth = 3)
{
    public void Move()
    {
        var bestMove = GetBestMove();
        if (bestMove != null)
            board.Move(bestMove);
    }

    private Move? GetBestMove()
    {
        if (board.IsEndGame)
            return null;

        var color = board.Turn;

        Move? bestMove = null;
        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = color.WorstScore();

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(depth, color.OppositeColor());
            board.Cancel();

            if ((color == PieceColor.White && score > bestScore) || // Maximize score for white
                (color == PieceColor.Black && score < bestScore)) // Minimize score for black
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(int depth, PieceColor color, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        if (depth == 0 || board.IsEndGame)
            return board.Evaluate();

        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = color.WorstScore();

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(depth - 1, color.OppositeColor(), alpha, beta);
            board.Cancel();

            if ((color == PieceColor.White && score > bestScore) || // Maximize score for white
                (color == PieceColor.Black && score < bestScore)) // Minimize score for black
                bestScore = score;

            if (color == PieceColor.White)
                alpha = Math.Max(alpha, score);
            else
                beta = Math.Min(beta, score);

            if (beta <= alpha)
                break;
        }

        return bestScore;
    }
}
