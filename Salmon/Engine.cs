using System.Diagnostics;
using Chess;

namespace Salmon;

public sealed class Engine
{
    public const string Name = "Salmon";
    public const string Author = "The Salmon team";

    private readonly ChessBoard _board;
    private readonly int _depth;

    public Engine(ChessBoard board, int depth = 3)
    {
        ArgumentNullException.ThrowIfNull(board);
        _board = board;
        _depth = depth;
    }

    public Move Move()
    {
        var move = GetBestMove();
        _board.Move(move);
        return move;
    }

    private Move GetBestMove()
    {
        Debug.Assert(!_board.IsEndGame);

        var color = _board.Turn;

        Move? bestMove = null;
        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = color.WorstScore();

        foreach (var move in _board.Moves())
        {
            _board.Move(move);
            var score = Minimax(_depth, color.OppositeColor());
            _board.Cancel();

            if ((color == PieceColor.White && score > bestScore) || // Maximize score for white
                (color == PieceColor.Black && score < bestScore)) // Minimize score for black
            {
                bestScore = score;
                bestMove = move;
            }
        }

        Debug.Assert(bestMove != null);

        return bestMove;
    }

    private int Minimax(int depth, PieceColor color, int alpha = int.MinValue, int beta = int.MaxValue)
    {
        if (depth == 0 || _board.IsEndGame)
            return _board.Evaluate();

        // Start with the worst score for the color, so the algorithm
        // knows when to minimize or maximize for the first move
        var bestScore = color.WorstScore();

        foreach (var move in _board.Moves())
        {
            _board.Move(move);
            var score = Minimax(depth - 1, color.OppositeColor(), alpha, beta);
            _board.Cancel();

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
