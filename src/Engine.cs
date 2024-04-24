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
        int bestScore = isWhite ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            int score = Minimax(depth, !isWhite);
            board.Cancel();

            if (isWhite && score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
            else if (!isWhite && score < bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(int depth, bool isWhite)
    {
        if (depth == 0 || board.IsEndGame)
            return board.Evaluate();

        int bestScore = isWhite ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            int score = Minimax(depth - 1, !isWhite);
            board.Cancel();

            if (isWhite && score > bestScore)
                bestScore = score;
            else if (!isWhite && score < bestScore)
                bestScore = score;
        }

        return bestScore;
    }
}
