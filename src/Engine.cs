using Chess;

namespace Salmon;

public class Engine(ChessBoard board, int depth = 2)
{
    public void Move()
    {
        var bestMove = GetBestMove(board, true);
        if (bestMove != null) board.Move(bestMove);
    }

    private Move? GetBestMove(ChessBoard board, bool isMaximizingPlayer)
    {
        if (board.IsEndGame) return null;

        Move? bestMove = null;
        var bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(board, depth, !isMaximizingPlayer);
            board.Cancel();

            if (isMaximizingPlayer && score > bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
            else if (!isMaximizingPlayer && score < bestScore)
            {
                bestScore = score;
                bestMove = move;
            }
        }

        return bestMove;
    }

    private int Minimax(ChessBoard board, int depth, bool isMaximizingPlayer)
    {
        if (depth == 0 || board.IsEndGame)
        {
            // Return the evaluation score if the maximum depth is reached or the game is over
            return board.Evaluate();
        }

        var bestScore = isMaximizingPlayer ? int.MinValue : int.MaxValue;

        foreach (var move in board.Moves())
        {
            board.Move(move);
            var score = Minimax(board, depth - 1, !isMaximizingPlayer);
            board.Cancel();

            if (isMaximizingPlayer && score > bestScore)
                bestScore = score;
            else if (!isMaximizingPlayer && score < bestScore)
                bestScore = score;
        }

        return bestScore;
    }
}