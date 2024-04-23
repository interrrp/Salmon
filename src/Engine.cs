using Chess;

namespace Salmon;

public class Engine
{
    private ChessBoard _board { get; }

    public Engine(ChessBoard board)
    {
        _board = board;
    }

    public void Move()
    {
        var bestMove = _board.Moves()
            .OrderBy(move =>
                (move.IsMate ? 3 : 0)
                + (move.IsCheck ? 2 : 0)
                + (move.HasValue ? 1 : 0)
            )
            .First();

        _board.Move(bestMove);
    }
}