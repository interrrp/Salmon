using Chess;

namespace Salmon;

public class Engine(ChessBoard board)
{
    public void Move()
    {
        var bestMove = board.Moves()
            .OrderBy(move =>
                (move.IsMate ? 3 : 0)
                + (move.IsCheck ? 2 : 0)
                + (move.HasValue ? 1 : 0)
            )
            .First();

        board.Move(bestMove);
    }
}