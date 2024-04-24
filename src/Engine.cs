using Chess;

namespace Salmon;

public class Engine(ChessBoard board)
{
    public void Move()
    {
        var bestMove = board.Moves()
            .OrderBy(move =>
            {
                board.Move(move);
                var eval = board.Evaluate();
                board.Cancel();
                return eval;
            })
            .First();

        board.Move(bestMove);
    }
}