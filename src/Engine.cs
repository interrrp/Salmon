using Chess;

namespace Salmon;

public class Engine(ChessBoard board, bool isWhite = false, int depth = 3)
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

        Move? bestMove = null;
        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = isWhite ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(depth, !isWhite);
            board.Cancel();

            if ((isWhite && score > bestScore) || // Maximize score for white
                (!isWhite && score < bestScore)) // Minimize score for black
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(int depth, bool isWhite, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        if (depth == 0 || board.IsEndGame)
            return board.Evaluate();

        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = isWhite ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(depth - 1, !isWhite, alpha, beta);
            board.Cancel();

            if ((isWhite && score > bestScore) || // Maximize score for white
                (!isWhite && score < bestScore)) // Minimize score for black
                bestScore = score;

            if (isWhite)
                alpha = Math.Max(alpha, score);
            else
                beta = Math.Min(beta, score);

            if (beta <= alpha)
                break;
        }

        return bestScore;
    }
}
