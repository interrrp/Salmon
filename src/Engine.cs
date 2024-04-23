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
        var moves = _board.Moves();
        var move = moves[Random.Shared.Next(moves.Length)];
        _board.Move(move);
    }
}